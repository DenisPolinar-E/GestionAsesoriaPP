using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.Follow.Request;
using GestionAsesoria.Operator.Application.DTOs.Follow.Response;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Application.Features.Follow.Queries;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionAsesoria.Operator.Api.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowController : ControllerBase
    {
        private readonly IFollowRepositoryAsync _repository; // Asumo que tienes esta interfaz

        public FollowController(IFollowRepositoryAsync repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult<List<ListFollowDto>>> GetAllFollowsAsync()
        {
            try
            {
                var result = await _repository.GetAllFollowsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("filtered")]
        public async Task<ActionResult<List<ListFollowDto>>> GetFilteredAsync(FollowFilterDto filters)
        {
            try
            {
                var result = await _repository.GetFilteredAsync(filters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

