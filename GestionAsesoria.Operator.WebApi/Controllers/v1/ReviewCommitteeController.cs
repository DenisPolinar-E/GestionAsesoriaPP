using GestionAsesoria.Operator.Application.DTOs.ReviewCommittee.Request;
using GestionAsesoria.Operator.Application.Features.ReviewCommittees.Commands;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReviewCommitteeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewCommitteeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Asigna un comité revisor a una práctica preprofesional finalizada.
        /// </summary>
        [HttpPost("assign")]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignReviewCommittee([FromBody] AssignReviewCommitteeDto dto)
        {
            var command = new AssignReviewCommitteeCommand { Request = dto };
            var result = await _mediator.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}