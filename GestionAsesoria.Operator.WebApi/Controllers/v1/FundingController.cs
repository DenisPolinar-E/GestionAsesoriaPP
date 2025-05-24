using GestionAsesoria.Operator.Application.Features.Fundings.Commands.Create;
using GestionAsesoria.Operator.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.WebApi.Controllers.v1
{
    public class FundingController : BaseApiController<FundingController>
    {
        /// <summary>
        /// Crear un nuevo financiamiento para un proyecto
        /// </summary>
        /// <param name="command">Comando con los datos del financiamiento</param>
        /// <returns>ID del financiamiento creado</returns>
        [Authorize(Policy = Permissions.Projects.Create)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateFundingCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
} 