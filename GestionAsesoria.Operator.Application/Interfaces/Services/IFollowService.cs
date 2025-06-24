using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.Follow.Request;
using GestionAsesoria.Operator.Application.DTOs.Follow.Response;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionAsesoria.Operator.Application.Interfaces.Services
{
    public interface IFollowService
    {
        Task<List<FollowDto>> GetAllFollowsAsync();
        Task<List<FollowDto>> GetProgresPppAsync();
        Task<List<FollowDto>> GetFilteredAsync(FilterFollowDto filters);
    }

    public class FollowService : IFollowService
    {
        private readonly IFollowRepositoryAsync _followRepositoryAsync;
        private readonly IMapper _mapper;

        public FollowService(IFollowRepositoryAsync followRepositoryAsync, IMapper mapper)
        {
            _followRepositoryAsync = followRepositoryAsync ?? throw new ArgumentNullException(nameof(followRepositoryAsync));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<FollowDto>> GetAllFollowsAsync()
        {
            return (await _followRepositoryAsync.GetAllFollowsAsync()).ToList();
        }

        public async Task<List<FollowDto>> GetProgresPppAsync()
        {
            var follows = await _followRepositoryAsync.GetProgresPppAsync();

            var result = follows.Select(f => new FollowDto
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
                ProgressPorcent = (f.EndDate > f.StartDate && f.StartDate <= DateTime.UtcNow)
                    ? Math.Min(100, Math.Round((double)(DateTime.UtcNow - f.StartDate).Days / (f.EndDate - f.StartDate).Days * 100, 2))
                    : 0
            }).ToList();

            return result;
        }


        public async Task<List<FollowDto>> GetFilteredAsync(FilterFollowDto filters)
        {
            var follows = await _followRepositoryAsync.GetFilteredAsync(filters);

            var result = follows.Select(f =>
            {
                var daysElapsed = (DateTime.UtcNow - f.StartDate).Days;
                var daysRemaining = (f.EndDate - DateTime.UtcNow).Days;
                var progress = f.DurationDays > 0 && f.StartDate <= DateTime.UtcNow && f.EndDate >= f.StartDate
                    ? Math.Min(100, Math.Round((double)daysElapsed / f.DurationDays * 100, 2))
                    : 0;

                return new FollowDto
                {
                    Id = f.Id,
                    FullNameInterId = f.FullNameInterId,
                    FullNameAdviserId = f.FullNameAdviserId,
                    FullNameCompanyId = f.FullNameCompanyId,
                    StartDate = f.StartDate,
                    EndDate = f.EndDate,
                    State = f.State,
                    DurationDays = f.DurationDays,
                    DaysElapsed = daysElapsed < 0 ? 0 : daysElapsed,
                    DaysRemaining = daysRemaining < 0 ? 0 : daysRemaining,
                    ProgressPorcent = progress
                };
            }).ToList();

            return result;
        }


    }
}