using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using GestionAsesoria.Operator.Application.Interfaces.Services;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext _context;

        public TeacherService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ActorTeacherCountDto>> GetTeachersWithPracticeCountAsync()
        {
            var result = await _context.Actor
                .Where(a => a.MainRoleId == 15)
                .Select(a => new ActorTeacherCountDto
                {
                    Id = a.Id,
                    FullName = a.FirstName + " " + a.SecondName,
                    PracticeCount = _context.PreProfessionalInternship.Count(p => p.AdviserId == a.Id),
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    IdentificationNumber = a.IdentificationNumber,
                    BranchName = _context.Actor
                     .Where(p => p.Id == a.ParentId)
                     .Select(p => p.FirstName + " " + p.SecondName)
                     .FirstOrDefault(),
                    StartDate = a.StartDate,
                    EndDate = a.EndDate
                })
                .ToListAsync();

            return result;
        }
    }
}
