using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly IActorCompanyService _actorService;

        public ActorController(IActorCompanyService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet("companiesWithPracticeCount")]
        public async Task<IActionResult> GetCompaniesWithPracticeCount()
        {
            var companies = await _actorService.GetCompaniesWithPracticeCountAsync();
            return Ok(companies);
        }
    }
}
