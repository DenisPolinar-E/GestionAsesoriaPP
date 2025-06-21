using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorTeacher;
using GestionAsesoria.Operator.Application.Features.Actors.Queries.GetActorTeacher;
using GestionAsesoria.Operator.Application.Features.Teachers.Queries.GetTeacherAvailability;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.WebApi.Controllers.Teachers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeacherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("availability")]
        public async Task<ActionResult<Result<IEnumerable<GetActorTeacherDto>>>> GetAvailability()
        {
            var result = await _mediator.Send(new GetAllTeacherAvailabilityQuery());
            return Ok(result.Data);
        }
    }
}
