using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorTeacher;
using GestionAsesoria.Operator.Application.Features.Actors.Queries.GetActorTeacher;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.Teachers.Queries.GetTeacherAvailability
{
    internal class GetAllTeacherAvailabilityQueryHandler : IRequestHandler<GetAllTeacherAvailabilityQuery, Result<IEnumerable<GetActorTeacherDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllTeacherAvailabilityQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<GetActorTeacherDto>>> Handle(GetAllTeacherAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var teachers = await _unitOfWork.TeacherPracticeRepository.GetTeacherAvailabilityAsync();
            return await Result<IEnumerable<GetActorTeacherDto>>.SuccessAsync(teachers);
        }
    }
}
