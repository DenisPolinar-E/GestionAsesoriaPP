using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorCompany;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class CompanyPracticeRepository : ICompanyPracticeRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;

        public CompanyPracticeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ActorCompanyDto>> GetCompanyPracticeCountsAsync()
        {
            var result = await _dbContext.Actor
                .Where(a => a.MainRoleId == 17) // Empresas
                .Select(a => new ActorCompanyDto
                {
                    Id = a.Id,
                    CompanyName = a.FirstName + " " + a.SecondName, // O usa un campo específico si tienes CompanyName
                    PracticeCount = _dbContext.RequestPPP.Count(r => r.CompanyId == a.Id),
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    IdentificationNumber = a.IdentificationNumber,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate
                })
                .ToListAsync();

            return result;
        }
    }
}
