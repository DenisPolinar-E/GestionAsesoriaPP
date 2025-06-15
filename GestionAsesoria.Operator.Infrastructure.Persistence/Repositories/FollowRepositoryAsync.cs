using GestionAsesoria.Operator.Application.DTOs.Follow.Request;
using GestionAsesoria.Operator.Application.DTOs.Follow.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class FollowRepositoryAsync : GenericRepositoryAsync<PreProfessionalInternship, int>, IFollowRepositoryAsync
    {
        private readonly DbSet<PreProfessionalInternship> _internships;
        private readonly ApplicationDbContext _context;

        public FollowRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _internships = dbContext.Set<PreProfessionalInternship>();
            _context = dbContext;
        }

        public async Task<List<ListFollowDto>> GetAllFollowsAsync()
        {
            return await _internships
                .Select(f => new ListFollowDto
                {
                    StudentName = f.StudentName,
                    StartDate = f.StartDate,
                    EndDate = f.EndDate,
                    Status = f.Status,
                    DurationDays = (f.EndDate - f.StartDate).Days
                })
                .ToListAsync();
        }

        public async Task<List<ListFollowDto>> GetProgresPppAsync()
        {
            return await _internships
                .Select(f => new ListFollowDto
                {
                    StudentName = f.StudentName,
                    StartDate = f.StartDate,
                    EndDate = f.EndDate,
                    Status = f.Status,
                    DurationDays = (f.EndDate - f.StartDate).Days,
                    DaysRemaining = (f.EndDate - DateTime.UtcNow).Days,
                    ProgressPorcent = (f.EndDate > f.StartDate && DateTime.UtcNow >= f.StartDate)
                        ? Math.Min(100, Math.Round((double)(DateTime.UtcNow - f.StartDate).Days / (f.EndDate - f.StartDate).Days * 100, 2))
                        : 0
                })
                .ToListAsync();
        }

        public async Task<List<ListFollowDto>> GetFilteredAsync(FollowFilterDto filters)
        {
            try
            {
                var query = _internships.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filters.StudentName))
                {
                    query = query.Where(p => p.StudentName.Contains(filters.StudentName));
                }

                if (!string.IsNullOrWhiteSpace(filters.Status))
                {
                    query = query.Where(p => p.Status.Contains(filters.Status));
                }

                return await query
                    .Select(f => new ListFollowDto
                    {
                        StudentName = f.StudentName,
                        StartDate = f.StartDate,
                        EndDate = f.EndDate,
                        Status = f.Status,
                        DurationDays = (f.EndDate - f.StartDate).Days
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al filtrar las prácticas preprofesionales: {ex.Message}", ex);
            }
        }
    }
}
