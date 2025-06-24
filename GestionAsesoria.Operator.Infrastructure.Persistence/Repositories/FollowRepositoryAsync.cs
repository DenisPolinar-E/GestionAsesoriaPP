using GestionAsesoria.Operator.Application.DTOs.Follow.Request;
using GestionAsesoria.Operator.Application.DTOs.Follow.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class FollowRepositoryAsync : IFollowRepositoryAsync
    {
        private readonly ApplicationDbContext _context;

        public FollowRepositoryAsync(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<FollowDto>> GetAllFollowsAsync()
        {
            return await _context.PreProfessionalInternship
                .Select(f => new FollowDto
                {
                    Id = f.Id,
                    FullNameInterId = f.FullNameInterId,
                    FullNameAdviserId = f.FullNameAdviserId,
                    FullNameCompanyId = f.FullNameCompanyId,
                    StartDate = f.StartDate,
                    EndDate = f.EndDate,
                    State = f.State,
                    DurationDays = (f.EndDate - f.StartDate).Days
                    // No necesitas calcular aquí, FollowDto lo hace
                })
                .ToListAsync();
        }

        public async Task<List<FollowDto>> GetProgresPppAsync()
        {
            return await _context.PreProfessionalInternship
                .Select(f => new FollowDto
                {
                    Id = f.Id,
                    FullNameInterId = f.FullNameInterId,
                    FullNameAdviserId = f.FullNameAdviserId,
                    FullNameCompanyId = f.FullNameCompanyId,
                    StartDate = f.StartDate,
                    EndDate = f.EndDate,
                    State = f.State,
                    DurationDays = (f.EndDate - f.StartDate).Days,
                    DaysElapsed = (DateTime.UtcNow - f.StartDate).Days,
                    DaysRemaining = (f.EndDate - DateTime.UtcNow).Days,
                    ProgressPorcent = (f.EndDate > f.StartDate && DateTime.UtcNow >= f.StartDate)
                        ? Math.Min(100, Math.Round((double)(DateTime.UtcNow - f.StartDate).Days / (f.EndDate - f.StartDate).Days * 100, 2))
                        : 0
                })
                .ToListAsync();
        }



        public async Task<List<FollowDto>> GetFilteredAsync(FilterFollowDto filters)
        {
            try
            {
                var query = _context.PreProfessionalInternship.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filters.FullNameInterId))
                {
                    query = query.Where(p => p.FullNameInterId.Contains(filters.FullNameInterId));
                }

                if (!string.IsNullOrWhiteSpace(filters.FullNameAdviserId))
                {
                    query = query.Where(p => p.FullNameAdviserId.Contains(filters.FullNameAdviserId));
                }

                if (!string.IsNullOrWhiteSpace(filters.State))
                {
                    query = query.Where(p => p.State.Contains(filters.State));
                }

                return await query.Select(f => new FollowDto
                {
                    Id = f.Id,
                    FullNameInterId = f.FullNameInterId,
                    FullNameAdviserId = f.FullNameAdviserId,
                    FullNameCompanyId = f.FullNameCompanyId,
                    StartDate = f.StartDate,
                    EndDate = f.EndDate,
                    State = f.State,
                    DurationDays = (f.EndDate - f.StartDate).Days
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al filtrar las prácticas preprofesionales: {ex.Message}", ex);
            }
        }

    }
}
