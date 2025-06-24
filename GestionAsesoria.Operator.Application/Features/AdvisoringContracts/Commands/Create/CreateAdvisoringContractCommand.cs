using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.AdvisoringContracts.Request;
using GestionAsesoria.Operator.Application.DTOs.Mail.Request;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Domain.Enums;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.AdvisoringContracts.Commands.Create
{
    public class CreateAdvisoringContractCommand : IRequest<Result<int>>
    {
        public CreateAdvisoringContractRequestDto Request { get; set; }
    }
    internal class CreateAdvisoringContractCommandHandler : IRequestHandler<CreateAdvisoringContractCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IMessageService _messageService;


        public CreateAdvisoringContractCommandHandler(
            IMapper mapper,
            IUnitOfWork<int> unitOfWork,
            IEmailService emailService,
            IMessageService messageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _messageService = messageService;

        }

        public async Task<Result<int>> Handle(CreateAdvisoringContractCommand command, CancellationToken cancellationToken)
        {
            try
            {
                ValidateInputParameters(command.Request);
                await ValidateExistingContracts(command.Request.EstudianteId);

                var (advisor, student) = await ValidateActors(command.Request);
                var researchGroup = await ValidateResearchGroup(command.Request);
                var serviceType = await GetRequiredMasterData();

                return await CreateContractAndRequest(command.Request, advisor, student, researchGroup, serviceType, cancellationToken);
            }
            catch (Exception ex)
            {
                string errorMessage = _messageService.GetDynamicMessage("AdvisoringContract", "Messages", "Error_Request");
                return await Result<int>.FailAsync($"{errorMessage}: {ex.Message}");
            }
        }

        private async Task<Actor> ValidateResearchGroup(CreateAdvisoringContractRequestDto request)
        {
            var researchGroup = await _unitOfWork.ActorRepository.GetResearchGroupWithDetailsAsync(
                request.ResearchGroupId,
                request.ResearchLineId,
                request.ResearchAreaId,
                request.DocenteId);

            if (researchGroup == null)
            {
                throw new InvalidOperationException(_messageService.GetDynamicMessage("AdvisoringContract", "Messages", "Filter_Tacher"));
            }

            return researchGroup;
        }

        private async Task ValidateExistingContracts(int estudianteId)
        {
            var existingContracts = await _unitOfWork.AdvisoringContractRepository.GetContractsByActorIdAsync(estudianteId);

            if (existingContracts?.Any() == true)
            {
                var hasActiveContract = existingContracts.Any(c => c.IsActived == true);

                if (hasActiveContract)
                {
                    string errorMessage = _messageService.GetDynamicMessage("AdvisoringContract", "Messages", "Active_Contract_Exists");
                    throw new InvalidOperationException(errorMessage);
                }
            }
        }

        private async Task<(Actor advisor, Actor student)> ValidateActors(CreateAdvisoringContractRequestDto request)
        {
            var student = await _unitOfWork.ActorRepository.GetActorWithDetailsAsync(request.EstudianteId);
            if (student == null || student.MainRoleId != 12)
            {
                throw new InvalidOperationException($"Estudiante con ID {request.EstudianteId} no encontrado o no es un estudiante válido");
            }

            var advisor = await _unitOfWork.ActorRepository.GetActorWithDetailsAsync(request.DocenteId);
            if (advisor == null || advisor.MainRoleId != 15)
            {
                throw new InvalidOperationException($"Docente con ID {request.DocenteId} no encontrado o no es un docente válido");
            }

            var teacherInGroup = await _unitOfWork.ActorRepository.GetResearchGroupWithDetailsAsync(
                request.ResearchGroupId,
                request.ResearchLineId,
                request.ResearchAreaId,
                request.DocenteId);

            if (teacherInGroup == null)
            {
                throw new InvalidOperationException($"Docente con ID {request.DocenteId} no pertenece al grupo de investigación especificado");
            }

            return (advisor, student);
        }

        private async Task<MasterDataValue> GetRequiredMasterData()
        {
            var serviceType = await _unitOfWork.MasterDataValueRepository.GetByCodeAsync("TESIS");

            if (serviceType == null)
            {
                throw new InvalidOperationException(_messageService.GetDynamicMessage("AdvisoringContract", "Messages", "AdvisoringContract_Status"));
            }

            return serviceType;
        }

        private async Task<Result<int>> CreateContractAndRequest(
            CreateAdvisoringContractRequestDto request,
            Actor advisor,
            Actor student,
            Actor researchGroup,
            MasterDataValue serviceType,
            CancellationToken cancellationToken)
        {
            try
            {
                var advisoringRequest = new AdvisoringRequest
                {
                    UserSubject = request.Subject,
                    UserMessage = request.Description,
                    DateRequest = DateTime.Now,
                    ServiceTypeId = serviceType.Id,
                    AdvisorActorId = advisor.Id,
                    UserActorId = advisor.Id,
                    RequesterActorId = student.Id,
                    AdvisoringRequestStatus = AdvisoringRequestStatus.PendingRequest,
                    ResearchGroupId = request.ResearchGroupId,
                    ResearchLineId = request.ResearchLineId,
                    ResearchAreaId = request.ResearchAreaId
                };

                var addedRequest = await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    var result = await _unitOfWork.AdvisoringRequestRepository.AddAsync(advisoringRequest);
                    await _unitOfWork.Commit(cancellationToken);
                    return result;
                }, IsolationLevel.ReadCommitted, cancellationToken);

                try
                {
                    await SendEmailNotification(advisor, researchGroup, request, student);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
                }

                string successMessage = _messageService.GetDynamicMessage("AdvisoringContract", "Messages", "AdvisoringContract_Success");
                return await Result<int>.SuccessAsync(addedRequest.Id);
            }
            catch (Exception ex)
            {
                string errorMessage = _messageService.GetDynamicMessage("AdvisoringContract", "Messages", "Error_Save");
                return await Result<int>.FailAsync($"{errorMessage}: {ex.Message}");
            }
        }

        private async Task SendEmailNotification(
            Actor advisor,
            Actor researchGroup,
            CreateAdvisoringContractRequestDto request,
            Actor student)
        {
            try
            {
                string emailBodyTemplate = _messageService.GetMessage("AdvisoringContract", "EmailTemplates", "NotificationBody") ??
                    "Estimado {0},\n\nSe ha creado un nuevo contrato de asesoría con los siguientes detalles:\n\nEstudiante: {1}\nDescripción: {2}\n\nSaludos cordiales.";

                string emailBody = string.Format(
                    emailBodyTemplate,
                    $"{advisor.FirstName ?? "Docente"}",
                    $"{student?.FirstName ?? "Estudiante"} {student?.SecondName ?? ""}",
                    request.Description
                );

                var mailRequest = new MailRequest
                {
                    To = advisor.Email,
                    Subject = request.Subject,
                    Body = emailBody,
                    From = _messageService.GetMessage("General", "EmailSettings", "DefaultSenderEmail"),
                    IsBodyHtml = false
                };

                await _emailService.SendEmailAsync(mailRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
            }
        }

        private void ValidateInputParameters(CreateAdvisoringContractRequestDto request)
        {
            if (request.ResearchGroupId <= 0 ||
                request.ResearchLineId <= 0 ||
                request.ResearchAreaId <= 0 ||
                request.DocenteId <= 0)
            {
                string errorMessage = _messageService.GetDynamicMessage("AdvisoringContract", "Messages", "Error_Parameters");
                throw new ArgumentException(errorMessage);
            }
        }
    }
}
