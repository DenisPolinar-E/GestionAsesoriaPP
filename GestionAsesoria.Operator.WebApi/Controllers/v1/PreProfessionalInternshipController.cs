using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternshipByAdvisoringContract.Request;
using GestionAsesoria.Operator.Application.Features.PreProfInternshipByContract.Commands.Assign;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreProfessionalInternshipController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PreProfessionalInternshipController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("assign-adviser")]
        public async Task<IActionResult> AssignAdviserAsync([FromBody] AssignAdviserToInternshipRequestDto dto)
        {
            var result = await _mediator.Send(new AssignAdviserToInternshipCommand { Model = dto });
            return Ok(result);
        }

    }
}