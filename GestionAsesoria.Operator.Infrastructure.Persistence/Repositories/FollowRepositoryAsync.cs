using GestionAsesoria.Operator.Application.DTOs.Follow.Request;
using GestionAsesoria.Operator.Application.DTOs.Follow.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

public class FollowRepositoryAsync : GenericRepositoryAsync<PreProfessionalInternship, int>, IFollowRepositoryAsync
{
    private readonly DbSet<PreProfessionalInternship> _internships;
    private readonly ApplicationDbContext _context;

    public FollowRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _internships = dbContext.Set<PreProfessionalInternship>();
        _context = dbContext;
    }

    private IQueryable<ListFollowDto> GetBaseQuery()
    {
        var now = DateTime.UtcNow;

        return _internships
            .Include(i => i.RequestPPP)
            .Select(f => new ListFollowDto
            {
                StudentName = f.RequestPPP.Student.FirstName + " " + f.RequestPPP.Student.SecondName,
                StartRequest = f.RequestPPP.StartRequest,
                EndRequest = f.RequestPPP.EndRequest,
                Status = f.RequestPPP.Status.Value,

                DurationDays = f.RequestPPP.EndRequest.HasValue && f.RequestPPP.StartRequest.HasValue
                    ? (f.RequestPPP.EndRequest.Value - f.RequestPPP.StartRequest.Value).Days
                    : 0,

                DaysElapsed = f.RequestPPP.StartRequest.HasValue
                    ? Math.Max((now - f.RequestPPP.StartRequest.Value).Days, 0)
                    : 0,

                DaysRemaining = f.RequestPPP.EndRequest.HasValue
                    ? Math.Max((f.RequestPPP.EndRequest.Value - now).Days, 0)
                    : 0
            });
    }


    public async Task<List<ListFollowDto>> GetAllFollowsAsync()
    {
        return await GetBaseQuery().ToListAsync();
    }

    public async Task<List<ListFollowDto>> GetFilteredAsync(FollowFilterDto filters)
    {
        var query = GetBaseQuery();

        if (!string.IsNullOrWhiteSpace(filters.StudentName))
        {
            var studentNameFilter = filters.StudentName.Trim().ToLower();
            query = query.Where(p => p.StudentName.ToLower().Contains(studentNameFilter));
        }

        if (!string.IsNullOrWhiteSpace(filters.Status))
        {
            var statusFilter = filters.Status.Trim().ToLower();
            query = query.Where(p => p.Status.ToLower().Contains(statusFilter));
        }

        return await query.ToListAsync();
    }



    public async Task<List<ListFollowDto>> GetProgresPppAsync()
    {
        return await GetBaseQuery().ToListAsync();
    }

}