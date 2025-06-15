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

            var teachers = await _dbContext.Actor
                .Where(a => a.MainRoleId == 15 && a.IsActived)
                .ToListAsync();

            var contracts = await _dbContext.AdvisoringContract
                .Where(c => c.IsActived)
                .ToListAsync();

            var result = teachers.Select(teacher =>
            {
                var teacherContracts = contracts.Where(c => c.AdvisorId == teacher.Id);

                var researchGroupName = teacherContracts
                    .Where(c => c.ResearchGroupId != null)
                    .Select(c =>
                        _dbContext.Actor
                            .Where(rg => rg.Id == c.ResearchGroupId && rg.MainRoleId == 11)
                            .Select(rg => rg.FirstName)
                            .FirstOrDefault()
                    )
                    .FirstOrDefault();

                var currentAdvisees = teacherContracts.Count();

                return new GetActorTeacherDto
                {
                    Id = teacher.Id,
                    Code = teacher.Code,  // asegúrate que Actor tiene "Code"
                    FullName = teacher.FirstName,  // cambia por el campo real de nombre completo
                    ResearchGroup = researchGroupName ?? "Sin grupo",
                    InstitutionalEmail = teacher.Email,  // cambia por el campo real
                    CurrentAdvisees = currentAdvisees,
                    Availability = currentAdvisees >= maxAdvisees ? "Full" : "Available"
                };
            }).ToList();

            return result;
        }

    }
}
