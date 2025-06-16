using GestionAsesoria.Operator.Application.Features.Companies.Queries.GetCompanyPracticeCounts;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorCompany;

namespace GestionAsesoria.Operator.WebApi.Controllers.Companies
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("practice-counts")]
        public async Task<ActionResult<Result<IEnumerable<ActorCompanyDto>>>> GetPracticeCounts()
        {
            var result = await _mediator.Send(new GetAllCompanyPracticeCountsQuery());
            return Ok(result.Data);
        }
    }
}
