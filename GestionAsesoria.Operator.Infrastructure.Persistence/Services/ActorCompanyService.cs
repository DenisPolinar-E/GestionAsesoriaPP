using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Services
{
    public class ActorCompanyService : IActorCompanyService
    {
        private readonly ApplicationDbContext _context;

        public ActorCompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompanyPracticeCountDto>> GetCompaniesWithPracticeCountAsync()
        {
            var result = await _context.Actor
                .Where(a => a.MainRoleId == 17)
                .Select(a => new CompanyPracticeCountDto
                {
                    Id = a.Id,
                    CompanyName = a.FirstName + " " + a.SecondName,
                    PracticeCount = _context.PreProfessionalInternship.Count(p => p.CompanyId == a.Id),
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
