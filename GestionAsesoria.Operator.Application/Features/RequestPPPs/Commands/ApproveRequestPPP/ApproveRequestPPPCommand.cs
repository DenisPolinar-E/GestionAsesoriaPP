using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.RequestPPP.Request;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System.Threading;

namespace GestionAsesoria.Operator.Application.Features.RequestPPPs.Commands.Approve
{
    public class ApproveRequestPPPCommand : IRequest<Result<int>>
    {
        public ApproveRequestPPPRequestDto Model { get; set; } = null!;
    }

    internal class ApproveRequestPPPCommandHandler : IRequestHandler<ApproveRequestPPPCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public ApproveRequestPPPCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(ApproveRequestPPPCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.RequestPPPRepository.GetByIdAsync(request.Model.RequestPPPId);
            var approvedStatus = await _unitOfWork.MasterDataValueRepository.GetByCodeAsync("REQ_APPROVED");

            if (entity == null || entity.CompanyRepresentativeId == 0)
                return await Result<int>.FailAsync("La solicitud no tiene asesor asignado.");

            entity.StatusId = approvedStatus.Id;

            await _unitOfWork.RequestPPPRepository.UpdateAsync(entity);

            var internship = new PreProfessionalInternship
            {
                RequestPPPId = entity.Id
            };

            await _unitOfWork.RequestPPPRepository.AddInternshipAsync(internship);
            await _unitOfWork.Commit(cancellationToken);

            return await Result<int>.SuccessAsync(internship.Id, "Solicitud aprobada y práctica registrada.");
        }
    }
}
namespace GestionAsesoria.Operator.Application.Features.RequestPPPs.Commands.ApproveRequestPPP
{
    internal class ApproveRequestPPPCommand
    {
        public int RequestPPPId { get; set; }

        public ApproveRequestPPPCommand(ApproveRequestPPPRequestDto dto)
        {
            RequestPPPId = dto.RequestPPPId;
        }
    }
}
