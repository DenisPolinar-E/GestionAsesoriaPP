using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternshipByAdvisoringContract.Request;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.PreProfInternshipByContract.Commands.Assign
{
    public class AssignAdviserToInternshipCommand : IRequest<Result<int>>
    {
        public AssignAdviserToInternshipRequestDto Model { get; set; } = null!;
    }

    internal class AssignAdviserToInternshipCommandHandler : IRequestHandler<AssignAdviserToInternshipCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public AssignAdviserToInternshipCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(AssignAdviserToInternshipCommand request, CancellationToken cancellationToken)
        {
            var internship = await _unitOfWork.PreProfessionalInternshipByAdvisoringContractRepository
                .GetByInternshipIdAsync(request.Model.PreProfessionalInternshipId);

            if (internship == null)
                return await Result<int>.FailAsync("No se encontró la práctica pre profesional.");

            if (internship.AdvisoringContractId != 0)
                return await Result<int>.FailAsync("Esta práctica ya tiene un asesor asignado.");

            internship.AdvisoringContractId = request.Model.AdvisoringContractId;

            await _unitOfWork.PreProfessionalInternshipByAdvisoringContractRepository.UpdateAsync(internship);
            await _unitOfWork.Commit(cancellationToken);

            return await Result<int>.SuccessAsync(internship.Id, "Asesor asignado correctamente.");
        }
    }
}
