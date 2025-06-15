using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request;
using GestionAsesoria.Operator.Application.DTOs.RequestPPP.Request;
using GestionAsesoria.Operator.Application.DTOs.RequestPPP.Response;
using GestionAsesoria.Operator.Application.Features.RequestPPPs.Commands;
using GestionAsesoria.Operator.Application.Features.RequestPPPs.Commands.Approve;
using GestionAsesoria.Operator.Application.Features.RequestPPPs.Commands.Assign;
using GestionAsesoria.Operator.Application.Features.RequestPPPs.Commands.Update;
using GestionAsesoria.Operator.Application.Features.RequestPPPs.Queries;
using GestionAsesoria.Operator.Application.Features.RequestPPPs.Queries.GetRequestPPP;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RequestPPPController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RequestPPPController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crea una nueva solicitud de PPP
        /// </summary>
        [HttpPost("create")]
        //[ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(Result<int>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromForm] CreateRequestPPPRequestDto requestDto)
        {
            var command = new CreateRequestPPPCommand
            {
                Request = requestDto,
                PlanDocument = new CreateDocumentCollectionRequestDto
                {
                    File = requestDto.Document
                }
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // GET: api/RequestPPP/list
        [HttpGet("list")]
        public async Task<ActionResult<List<ListRequestPPPDto>>> GetAllForListAsync()
        {
            var result = await _mediator.Send(new GetAllRequestPPPForListQuery());
            return Ok(result);
        }

        [HttpPost("assign-adviser")]
        public async Task<IActionResult> AssignAdviserAsync([FromBody] AssignAdviserToRequestPPPRequestDto dto)
        {
            var result = await _mediator.Send(new AssignAdviserToRequestPPPCommand { Model = dto });
            return Ok(result);
        }

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveAsync([FromBody] ApproveRequestPPPRequestDto dto)
        {
            var result = await _mediator.Send(new ApproveRequestPPPCommand { Model = dto });
            return Ok(result);
        }
        [HttpGet("getState/{id}")]
        public async Task<ActionResult<StateRequestPPPByIdResponseDto>> GetStateRequestPPPById(int id)
        {
            var result = await _mediator.Send(new GetStateRequestPPPByIdQuery(id));
            return Ok(result);

        }
        [HttpPut("updateState")]
        public async Task<IActionResult> UpdateStateRequestPPPById([FromBody] UpdateStateRequestPPPByIdDto dto)
        {
            var result = await _mediator.Send(new UpdateStateRequestPPPByIdCommand { StateRequest=dto });
            return Ok(result);

        }
    }
}
