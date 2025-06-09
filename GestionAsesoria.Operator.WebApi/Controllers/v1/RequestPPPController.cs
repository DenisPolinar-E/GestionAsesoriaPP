using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request;
using GestionAsesoria.Operator.Application.Features.RequestPPPs.Commands;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
