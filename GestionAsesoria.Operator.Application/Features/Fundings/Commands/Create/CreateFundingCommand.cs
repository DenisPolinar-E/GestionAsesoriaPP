using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.Funding.Request;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities.ProjectIDI;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.Fundings.Commands.Create
{
    public class CreateFundingCommand : IRequest<Result<int>>
    {
        public CreateFundingRequestDto Request { get; set; }
    }

    internal class CreateFundingCommandHandler : IRequestHandler<CreateFundingCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;

        public CreateFundingCommandHandler(
            IMapper mapper,
            IUnitOfWork<int> unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateFundingCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Verificar que el proyecto exista
                var project = await _unitOfWork.ProjectRepository.GetByIdAsync(command.Request.ProjectId);
                if (project == null)
                {
                    return await Result<int>.FailAsync($"El proyecto con ID {command.Request.ProjectId} no existe.");
                }

                // Mapear el DTO a la entidad
                var funding = _mapper.Map<Funding>(command.Request);
                
                // Persistir la entidad
                await _unitOfWork.FundingRepository.AddAsync(funding);
                await _unitOfWork.Commit(cancellationToken);

                return await Result<int>.SuccessAsync(funding.Id, "Financiamiento añadido exitosamente.");
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync($"Error al añadir financiamiento: {ex.Message}");
            }
        }
    }
} 