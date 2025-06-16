using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorTeacher;

namespace GestionAsesoria.Operator.Application.Features.Actors.Queries.GetActorTeacher
{
    public class GetAllTeacherAvailabilityQuery : IRequest<Result<IEnumerable<GetActorTeacherDto>>>
    {
    }
}
