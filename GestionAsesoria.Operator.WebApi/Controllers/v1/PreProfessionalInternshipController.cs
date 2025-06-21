using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternshipByAdvisoringContract.Request;
using GestionAsesoria.Operator.Application.Features.PreProfInternshipByContract.Commands.Assign;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
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
        private readonly IMediator _mediator;
        private AssignAdviserToInternshipRequestDto dto;

        public PreProfessionalInternshipController(IPreProfessionalInternshipRepositoryAsync repository,IMediator mediator) 
        {
            _repository = repository;
            _mediator = mediator; 
        }

        [HttpPost("assign-adviser")]
        public async Task<IActionResult> AssignAdviserAsync([FromBody] AssignAdviserToInternshipRequestDto dto)
        {
            try
            {
                var result = await _mediator.Send(new AssignAdviserToInternshipCommand { Model = dto });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<PreProfessionalInternshipDto>>> GetAll()
        {
            try
            {
                var data = await _repository.GetAllPppAsync();
                return Ok(data);
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
