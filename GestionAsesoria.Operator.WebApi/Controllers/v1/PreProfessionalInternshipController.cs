using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreProfessionalInternshipController : ControllerBase
    {
        private readonly IPreProfessionalInternshipRepositoryAsync _repository;

        public PreProfessionalInternshipController(IPreProfessionalInternshipRepositoryAsync repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PreProfessionalInternshipDto>>> GetAll()
        {
            try
            {
                var result = await _repository.GetAllPppAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("filtered")]
        public async Task<ActionResult<List<FilterPreProfessionalInternshipDto>>> GetFiltered([FromBody] FilterPreProfessionalInternshipDto filters)
        {
            try
            {
                var result = await _repository.GetFilteredPppAsync(filters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
