using GestionAsesoria.Operator.Application.DTOs.RequestPPP.Request;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.RequestPPPs.Commands.Update
{
    public class UpdateStateRequestPPPByIdCommand: IRequest<Result<int>>
    {
        public UpdateStateRequestPPPByIdDto StateRequest { get; set; }
    }

    internal class UpdateStateRequestPPPByIdCommandHandler
        : IRequestHandler<UpdateStateRequestPPPByIdCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public UpdateStateRequestPPPByIdCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(
            UpdateStateRequestPPPByIdCommand command,
            CancellationToken cancellationToken)
        {
            var dto = command.StateRequest;

            // Llama al repositorio para actualizar el estado
            var actualizado =
                await _unitOfWork
                      .RequestPPPRepository.UpdateStateRequestPPPByIdAsync(dto.Id, dto.StatusId);

            if (!actualizado)
                // Si no encontró la entidad o falló la actualización
                return await Result<int>
                    .FailAsync("No se encontró la solicitud o no se pudo actualizar el estado.");

            // Devuelvo el Id de la solicitud actualizada
            return await Result<int>
                .SuccessAsync(dto.Id, "Estado actualizado correctamente.");
        }
    }
}
