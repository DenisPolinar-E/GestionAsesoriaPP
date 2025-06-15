using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorTeacher;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories
{
    public interface ITeacherPracticeRepositoryAsync
    {
        Task<IEnumerable<GetActorTeacherDto>> GetTeacherAvailabilityAsync();
    }
}
