using System;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionAsesoria.Operator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreProfessionalInternshipController : ControllerBase
    {
        private readonly IPreProfessionalInternshipService _service;

        public PreProfessionalInternshipController(IPreProfessionalInternshipService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al obtener las prácticas.", Details = ex.Message });
            }
        }

        [HttpPost("filtered")]
        public async Task<IActionResult> GetFiltered([FromBody] FilterPreProfessionalInternshipDto filters)
        {
            try
            {
                var result = await _service.GetFilteredAsync(filters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al filtrar las prácticas.", Details = ex.Message });
            }
        }
    }
}