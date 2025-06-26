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
                .ThenInclude(r => r.Student)
            .Include(i => i.PreProfessionalInternshipContracts)
                .ThenInclude(c => c.AdvisoringContract)
                .ThenInclude(ac => ac.AdvisoringRequest)
                .ThenInclude(ar => ar.AdvisorActor)
            .Select(f => new ListFollowDto
            {
                StudentName = f.RequestPPP.Student != null
                    ? f.RequestPPP.Student.FirstName + " " + f.RequestPPP.Student.SecondName
                    : "Desconocido",
                StudentEmail = f.RequestPPP.Student != null ? f.RequestPPP.Student.Email : string.Empty,
                AdvisorName = f.PreProfessionalInternshipContracts.Any() &&
                              f.PreProfessionalInternshipContracts.FirstOrDefault().AdvisoringContract != null &&
                              f.PreProfessionalInternshipContracts.FirstOrDefault().AdvisoringContract.AdvisoringRequest != null &&
                              f.PreProfessionalInternshipContracts.FirstOrDefault().AdvisoringContract.AdvisoringRequest.AdvisorActor != null
                    ? f.PreProfessionalInternshipContracts.FirstOrDefault().AdvisoringContract.AdvisoringRequest.AdvisorActor.FirstName + " " +
                      f.PreProfessionalInternshipContracts.FirstOrDefault().AdvisoringContract.AdvisoringRequest.AdvisorActor.SecondName
                    : "Desconocido",
                AdvisorEmail = f.PreProfessionalInternshipContracts.Any() &&
                               f.PreProfessionalInternshipContracts.FirstOrDefault().AdvisoringContract != null &&
                               f.PreProfessionalInternshipContracts.FirstOrDefault().AdvisoringContract.AdvisoringRequest != null &&
                               f.PreProfessionalInternshipContracts.FirstOrDefault().AdvisoringContract.AdvisoringRequest.AdvisorActor != null
                    ? f.PreProfessionalInternshipContracts.FirstOrDefault().AdvisoringContract.AdvisoringRequest.AdvisorActor.Email
                    : string.Empty,
                StartPreProfessionalPractice = f.RequestPPP.StartPreProfessionalPractice,
                EndPreProfessionalPractice = f.RequestPPP.EndPreProfessionalPractice,
                Status = f.RequestPPP.Status != null ? f.RequestPPP.Status.Value : "Sin estado",
                DurationDays = f.RequestPPP.EndPreProfessionalPractice.HasValue && f.RequestPPP.StartPreProfessionalPractice.HasValue
                    ? (f.RequestPPP.EndPreProfessionalPractice.Value - f.RequestPPP.StartPreProfessionalPractice.Value).Days
                    : 0,
                DaysElapsed = f.RequestPPP.StartPreProfessionalPractice.HasValue
                    ? Math.Max((now - f.RequestPPP.StartPreProfessionalPractice.Value).Days, 0)
                    : 0,
                DaysRemaining = f.RequestPPP.EndPreProfessionalPractice.HasValue
                    ? Math.Max((f.RequestPPP.EndPreProfessionalPractice.Value - now).Days, 0)
                    : 0,
                ProgressPercentage = f.RequestPPP.StartPreProfessionalPractice.HasValue && f.RequestPPP.EndPreProfessionalPractice.HasValue
                    ? Math.Min((now - f.RequestPPP.StartPreProfessionalPractice.Value).TotalDays /
                               (f.RequestPPP.EndPreProfessionalPractice.Value - f.RequestPPP.StartPreProfessionalPractice.Value).TotalDays * 100, 100)
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

    public async Task<List<ListFollowDto>> GetInternshipsExpiringIn7DaysAsync()
    {
        var now = DateTime.UtcNow.Date;
        var targetDate = now.AddDays(7);

        var query = GetBaseQuery()
            .Where(p => p.EndPreProfessionalPractice.HasValue &&
                        p.EndPreProfessionalPractice.Value.Date >= now &&
                        p.EndPreProfessionalPractice.Value.Date <= targetDate &&
                        p.Status.ToLower() == "activa");

        return await query.ToListAsync();
    }

    public async Task<List<ListFollowDto>> GetProgresPppAsync()
    {
        return await GetBaseQuery().ToListAsync();
    }

    public async Task UpdateExpiredInternshipsAsync()
    {
        var now = DateTime.UtcNow.Date;
        var internships = await _internships
            .Include(i => i.RequestPPP)
            .Where(i => i.RequestPPP.EndPreProfessionalPractice.HasValue &&
                        i.RequestPPP.EndPreProfessionalPractice.Value.Date < now &&
                        i.RequestPPP.Status.Value.ToLower() == "activa")
            .ToListAsync();

        foreach (var internship in internships)
        {
            internship.RequestPPP.Status.Value = "vencida";
        }

        await _context.SaveChangesAsync();
    }
}