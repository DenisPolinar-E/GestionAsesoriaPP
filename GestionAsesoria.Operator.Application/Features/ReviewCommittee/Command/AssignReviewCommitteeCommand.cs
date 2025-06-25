using GestionAsesoria.Operator.Application.DTOs.ReviewCommittee.Request;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Shared.Constants.Role;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.ReviewCommittees.Commands
{
    public class AssignReviewCommitteeCommand : IRequest<Result<int>>
    {
        public AssignReviewCommitteeDto Request { get; set; }
    }

    internal class AssignReviewCommitteeCommandHandler : IRequestHandler<AssignReviewCommitteeCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public AssignReviewCommitteeCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(AssignReviewCommitteeCommand command, CancellationToken cancellationToken)
        {
            var dto = command.Request;
            var (isValid, errorMessage) = dto.Validate();
            if (!isValid)
                return Result<int>.Fail(errorMessage);

            // Obtener la práctica preprofesional
            var internship = await _unitOfWork.Repository<PreProfessionalInternship>()
                .GetByIdAsync(dto.PreProfessionalInternshipId);
            if (internship == null)
                return Result<int>.Fail("La práctica preprofesional no existe.");

            // Obtener la solicitud PPP asociada para validar la fecha de fin
            var requestPPP = await _unitOfWork.Repository<RequestPPP>()
                .GetByIdAsync(internship.RequestPPPId); // Asumimos que PreProfessionalInternship tiene RequestPPPId
            if (requestPPP == null)
                return Result<int>.Fail("No se encontró la solicitud asociada.");

            // Validar que la práctica esté finalizada
            if (requestPPP.EndPreProfessionalPractice.HasValue && requestPPP.EndPreProfessionalPractice.Value > DateTime.Now)
                return Result<int>.Fail("La práctica no está finalizada.");

            var advisorRequest = await _unitOfWork.AdvisoringRequestRepository
               .Entities
               .Where(ar => ar.RequesterActorId == internship.RequestPPPId)
               .FirstOrDefaultAsync(cancellationToken);

            if (advisorRequest == null || advisorRequest.AdvisorActorId == 0)
                return Result<int>.Fail("La práctica no tiene un asesor asignado.");


            var advisorId = advisorRequest.AdvisorActorId;
            var researchGroupId = advisorRequest.ResearchGroupId;
            if (!researchGroupId.HasValue)
                return Result<int>.Fail("No se encontró el grupo de investigación asociado a la solicitud.");

            // Obtener el Actor que representa el grupo de investigación
            var researchGroupActor = await _unitOfWork.Repository<Actor>()
                .GetByIdAsync(researchGroupId.Value);
            if (researchGroupActor == null)
                return Result<int>.Fail("No se encontró el grupo de investigación asociado.");

            // Obtener el ID del rol por su nombre
            var reviewCommitteeRole = await _unitOfWork.Repository<Role>()
                .Entities
                .Where(r => r.Name == "Comité Revisor") // Usa el nombre real
                .FirstOrDefaultAsync();

            if (reviewCommitteeRole == null)
                return Result<int>.Fail("No se encontró el rol de comité revisor en el sistema.");

            var reviewCommitteeRoleId = reviewCommitteeRole.Id;


            


            // Asignar comité revisor a la práctica preprofesional
            internship.ReviewCommitteePrimaryId = dto.ReviewerTeacherIds[0];
            internship.ReviewCommitteeSecondaryId = dto.ReviewerTeacherIds[1];
            await _unitOfWork.Repository<PreProfessionalInternship>().UpdateAsync(internship);
            await _unitOfWork.Commit(cancellationToken);

            return Result<int>.Success(internship.Id, "Comité revisor asignado correctamente.");
        }
    }
}