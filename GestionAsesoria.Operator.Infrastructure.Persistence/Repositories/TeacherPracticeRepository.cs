using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorTeacher;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class TeacherPracticeRepository : ITeacherPracticeRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;

        public TeacherPracticeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GetActorTeacherDto>> GetTeacherAvailabilityAsync()
        {
            int maxAdvisees = 5;

            var result = await (
                from teacher in _dbContext.Actor
                where teacher.MainRoleId == 15 && teacher.IsActived
                join parent in _dbContext.Actor
                    on teacher.ParentId equals parent.Id into parentJoin
                from parent in parentJoin.DefaultIfEmpty()
                select new GetActorTeacherDto
                {
                    Id = teacher.Id,
                    Code = teacher.Code,
                    FullName = (teacher.FirstName + " " +
                                (teacher.SecondName ?? "") + " " +
                                (teacher.ThirdName ?? "")).Trim(),
                    ResearchGroup = parent != null ? parent.FirstName : "Sin grupo",
                    InstitutionalEmail = teacher.Email,
                    CurrentAdvisees = _dbContext.AdvisoringContract
                        .Count(c => c.AdvisorId == teacher.Id && c.IsActived),
                    Availability = "" // lo calculamos después
                }
            ).ToListAsync();

            // Calcular disponibilidad en memoria
            foreach (var item in result)
            {
                item.Availability = item.CurrentAdvisees >= maxAdvisees ? "Full" : "Available";
            }

            return result;
        }


    }
}
