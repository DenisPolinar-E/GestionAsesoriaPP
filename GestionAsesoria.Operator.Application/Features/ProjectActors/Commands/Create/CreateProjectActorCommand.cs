using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.ProjectActor.Request;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities.ProjectIDI;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.ProjectActors.Commands.Create
{
    public class CreateProjectActorCommand : IRequest<Result<int>>
    {
        public CreateProjectActorRequestDto Request { get; set; }
    }

    internal class CreateProjectActorCommandHandler : IRequestHandler<CreateProjectActorCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;

        public CreateProjectActorCommandHandler(
            IMapper mapper,
            IUnitOfWork<int> unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateProjectActorCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Mapear el DTO a la entidad
                var projectActor = _mapper.Map<ProjectActor>(command.Request);
                
                // Persistir la entidad
                await _unitOfWork.ProjectActorRepository.AddAsync(projectActor);
                await _unitOfWork.Commit(cancellationToken);

                return await Result<int>.SuccessAsync(projectActor.Id, "Participante añadido al proyecto exitosamente.");
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync($"Error al añadir participante al proyecto: {ex.Message}");
            }
        }
    }
} 