using GestionAsesoria.Operator.Application.DTOs.Follow.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.Interfaces.Common;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq; // Añadir esta línea
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowController : ControllerBase
    {
        private readonly IFollowRepositoryAsync _followRepository;
        private readonly IFollowService _followService;

        public FollowController(IFollowRepositoryAsync followRepository, IFollowService followService)
        {
            _followRepository = followRepository;
            _followService = followService;
        }

        [HttpGet("follow")]
        public async Task<IActionResult> GetAllFollowsAsync()
        {
            var internship = await _followRepository.GetProgresPppAsync();

            var response = internship.Select(f => new
            {
                f.Id,
                f.FullNameInterId,
                f.FullNameAdviserId,
                f.FullNameCompanyId,
                f.StartDate,
                f.EndDate,
                f.State,
                f.DurationDays,
                DaysElapsed = (DateTime.UtcNow - f.StartDate).Days,
                DaysRemaining = (f.EndDate - DateTime.UtcNow).Days,
                ProgressPercent = f.DurationDays > 0
                    ? Math.Min(100, Math.Round((double)(DateTime.UtcNow - f.StartDate).Days / f.DurationDays * 100, 2))
                    : 0
            }).ToList();

            return Ok(response);
        }

        [HttpPost("filtered")]
        public async Task<IActionResult> GetFiltered([FromBody] FilterFollowDto filters)
        {
            try
            {
                var result = await _followService.GetFilteredAsync(filters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al filtrar.", Details = ex.Message });
            }
        }


    }
}