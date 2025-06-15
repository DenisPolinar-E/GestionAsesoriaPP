using System.Collections.Generic;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.Follow.Request;
using GestionAsesoria.Operator.Application.DTOs.Follow.Response;
using GestionAsesoria.Operator.Application.Features.Follow.Queries;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionAsesoria.Operator.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FollowController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FollowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Result<List<ListFollowDto>>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllFollowsQuery());
            return Ok(result);
        }

        [HttpPost("filter")]
        public async Task<ActionResult<Result<List<ListFollowDto>>>> GetFiltered([FromBody] FollowFilterDto filters)
        {
            var query = new GetFilteredFollowsQuery(filters);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
