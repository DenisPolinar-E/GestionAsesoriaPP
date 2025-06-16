using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request;
using GestionAsesoria.Operator.Application.Features.DocumentCollections.Commands.Create;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.ConfigParameters.Container;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Tsp.Sigescom.Config;

namespace GestionAsesoria.Operator.Application.Features.RequestPPPs.Commands
{
    public class CreateRequestPPPCommand : IRequest<Result<int>>
    {
        public CreateRequestPPPRequestDto Request { get; set; }
        public CreateDocumentCollectionRequestDto PlanDocument { get; set; }
    }

    internal class CreateRequestPPPCommandHandler : IRequestHandler<CreateRequestPPPCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;
        // readonly SettingsContainer _settingsContainer;

        public CreateRequestPPPCommandHandler(
            IMapper mapper,
            IUnitOfWork<int> unitOfWork,
            IMediator mediator,
            IWebHostEnvironment env)

        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _env = env;
            //_settingsContainer = LocalSettingContainer.Get();
        }

        public async Task<Result<int>> Handle(CreateRequestPPPCommand command, CancellationToken cancellationToken)
        {
            var dto = command.Request;

            // === Obtener IDs relacionados ===
            var estudianteRoleId = await _unitOfWork.RoleRepository.GetIdByNameAsync("Estudiante");
            var empleadoRoleId = await _unitOfWork.RoleRepository.GetIdByNameAsync("Empleado");
            var empresaRoleId = await _unitOfWork.RoleRepository.GetIdByNameAsync("Empresa");

            var personaNaturalTypeId = (await _unitOfWork.ActorTypeRepository.GetIdByNameAsync("Persona Natural")).Value;
            var personaJuridicaTypeId = (await _unitOfWork.ActorTypeRepository.GetIdByNameAsync("Persona Jurídica")).Value;

            var StudentTypeId = (await _unitOfWork.MasterDataValueRepository.GetByCodeAsync("EXTORDESTDP")).Id;
            var OtrosTypeId = (await _unitOfWork.MasterDataValueRepository.GetByCodeAsync("OTROS")).Id;
            
            

            var facultadActorId = (await _unitOfWork.ActorRepository.GetByCodeAsync("FAC001")).Id;

            // === Crear o reutilizar actores ===
            var studentId = await GetOrCreateActorIdAsync(dto.StudentDni, new Actor
            {
                FirstName = dto.StudentFirstName,
                SecondName = dto.StudentLastName,
                Code = dto.StudentCode,
                IdentificationNumber = dto.StudentDni,
                Email = dto.StudentEmail,
                PhoneNumber = dto.StudentPhone,
                ClassifyActor = dto.StudentGender,
                StartDate = DateTime.UtcNow,
                IsActived = true,
                ActorTypeId = personaNaturalTypeId,
                IdentificationTypeId = StudentTypeId,
                ParentId = facultadActorId,
                MainRoleId = estudianteRoleId
            }, cancellationToken);

            var companyId = await GetOrCreateActorIdAsync(dto.CompanyRuc, new Actor
            {
                FirstName = dto.CompanyName,
                ThirdName = dto.CompanyAddress,
                IdentificationNumber = dto.CompanyRuc,
                ClassifyActor = "Entidad Externa - Empresa "+ dto.CompanyType,
                StartDate = DateTime.UtcNow,
                IsActived = true,
                ActorTypeId = personaJuridicaTypeId,
                IdentificationTypeId = OtrosTypeId,
                MainRoleId = empresaRoleId
            }, cancellationToken);

            var companyRepresentativeId = await GetOrCreateActorIdAsync(dto.CompanyRepresentativeDni, new Actor
            {
                FirstName = dto.CompanyRepresentativeFirstName,
                SecondName = dto.CompanyRepresentativeLastName,
                IdentificationNumber = dto.CompanyRepresentativeDni,
                ClassifyActor = dto.CompanyRepresentativeGender,
                StartDate = DateTime.UtcNow,
                IsActived = true,
                ActorTypeId = personaNaturalTypeId,
                IdentificationTypeId = OtrosTypeId,
                ParentId = companyId,
                MainRoleId = empleadoRoleId
            }, cancellationToken);

            var representativeId = await GetOrCreateActorIdAsync(dto.RepresentativeDni, new Actor
            {
                FirstName = dto.RepresentativeFirstName,
                SecondName = dto.RepresentativeLastName,
                IdentificationNumber = dto.RepresentativeDni,
                ClassifyActor = dto.RepresentativeGender,
                Email = dto.RepresentativeEmail,
                PhoneNumber = dto.RepresentativePhone,
                ThirdName = dto.RepresentativePosition,
                StartDate = DateTime.UtcNow,
                IsActived = true,
                ActorTypeId = personaNaturalTypeId,
                IdentificationTypeId = OtrosTypeId,
                ParentId = companyId,
                MainRoleId = empleadoRoleId
            }, cancellationToken);


            // === Guardar documento si se envió ===
            int documentId = 0;
            if (command.PlanDocument?.File != null)
            {
                documentId = await GuardarDocumentoPlanAsync(command.PlanDocument, $"Plan de prácticas: {dto.Title}", studentId, cancellationToken);
            }

            // === Crear solicitud PPP ===
            var status = await _unitOfWork.MasterDataValueRepository.GetByCodeAsync("REQ_PENDING");
            //var statePending = _settingsContainer.LocalRequestPPPSettings.StateRequestPPPId;


            var requestPPP = _mapper.Map<RequestPPP>(dto);
            requestPPP.StudentId = studentId;
            requestPPP.CompanyId = companyId;
            requestPPP.RepresentativeId = representativeId;
            requestPPP.DocumentCollectionId = documentId;
            requestPPP.CompanyRepresentativeId = companyRepresentativeId;
            requestPPP.StatusId = status.Id;
            requestPPP.ResearchAreaId = dto.ResearchAreaId;
            requestPPP.StartDate = DateTime.UtcNow;
            

            await _unitOfWork.RequestPPPRepository.AddAsync(requestPPP);
            await _unitOfWork.Commit(cancellationToken);

            return await Result<int>.SuccessAsync(requestPPP.Id, "Solicitud de PPP registrada correctamente.");
        }






        private async Task<int> GetOrCreateActorIdAsync(string dni, Actor newActor, CancellationToken cancellationToken)
        {
            var existing = await _unitOfWork.ActorRepository.GetByIdentificationNumberAsync(dni);
            if (existing != null)
            {
                return existing.Id;
            }

            await _unitOfWork.ActorRepository.AddAsync(newActor);
            await _unitOfWork.Commit(cancellationToken);
            return newActor.Id;
        }



        private async Task<int> GuardarDocumentoPlanAsync(CreateDocumentCollectionRequestDto plan, string titulo, int actorId, CancellationToken cancellationToken)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var extension = Path.GetExtension(plan.File.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await plan.File.CopyToAsync(stream);
            }

            var document = new DocumentCollection
            {
                Title = plan.Title ?? titulo,
                Description = plan.Description ?? "Documento del plan de prácticas preprofesionales",
                FilePath = filePath,
                FileSize = plan.File.Length,
                UploadDate = DateTime.UtcNow,
                OnlineUrl = $"/uploads/{uniqueFileName}",
                DocumentTypeId = (await _unitOfWork.MasterDataValueRepository.GetByCodeAsync("PLAN_PPP")).Id,
                UploadedByActorId = actorId
            };

            await _unitOfWork.DocumentCollectionRepository.AddAsync(document);
            await _unitOfWork.Commit(cancellationToken);

            return document.Id;
        }
    }
}
