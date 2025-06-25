// GestionAsesoria.Operator.WebApi/Controllers/v1/PresentationLetterController.cs
using GestionAsesoria.Operator.Application.DTOs.PresentationLetter.Request;
using GestionAsesoria.Operator.Application.DTOs.PresentationLetter.Response;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PresentationLetterController : ControllerBase
    {
        private readonly IPresentationLetterService _service;

        public PresentationLetterController(IPresentationLetterService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePresentationLetterDto dto)
        {
            var id = await _service.CreatePresentationLetterAsync(dto);
            return Ok(new { id });
        }

        [HttpGet("by-request/{requestPPPId}")]
        public async Task<ActionResult<PresentationLetterDto>> GetByRequestPPPId(int requestPPPId)
        {
            var result = await _service.GetByRequestPPPIdAsync(requestPPPId);
            return Ok(result);
        }
    }
}