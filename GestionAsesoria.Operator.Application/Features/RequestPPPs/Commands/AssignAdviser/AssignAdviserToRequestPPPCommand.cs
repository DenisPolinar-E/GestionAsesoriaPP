using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.RequestPPP.Request;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System.Threading;

namespace GestionAsesoria.Operator.Application.Features.RequestPPPs.Commands.Assign
{
    public class AssignAdviserToRequestPPPCommand : IRequest<Result<int>>
    {
        public AssignAdviserToRequestPPPRequestDto Model { get; set; } = null!;
    }

    internal class AssignAdviserToRequestPPPCommandHandler : IRequestHandler<AssignAdviserToRequestPPPCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public AssignAdviserToRequestPPPCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(AssignAdviserToRequestPPPCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.RequestPPPRepository.GetByIdAsync(request.Model.RequestPPPId);

            if (entity == null)
                return await Result<int>.FailAsync("No se encontró la solicitud.");

            var approvedStatus = await _unitOfWork.MasterDataValueRepository.GetByCodeAsync("REQ_APPROVED");

            if (entity.StatusId != approvedStatus.Id)
                return await Result<int>.FailAsync("La solicitud aún no ha sido aprobada.");

            var student = await _unitOfWork.ActorRepository.GetByCodeAsync(request.Model.StudentCode);
            if (student == null || student.Id != entity.StudentId)
                return await Result<int>.FailAsync("El estudiante no coincide con la solicitud.");

            entity.CompanyRepresentativeId = request.Model.AdviserId;

            await _unitOfWork.RequestPPPRepository.UpdateAsync(entity);
            await _unitOfWork.Commit(cancellationToken);

            return await Result<int>.SuccessAsync(entity.Id, "Asesor asignado correctamente.");
        }

    }
}
