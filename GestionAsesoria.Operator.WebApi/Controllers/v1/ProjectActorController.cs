using GestionAsesoria.Operator.Application.Features.ProjectActors.Commands.Create;
using GestionAsesoria.Operator.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.WebApi.Controllers.v1
{
    public class ProjectActorController : BaseApiController<ProjectActorController>
    {
        /// <summary>
        /// Crear un nuevo participante para un proyecto
        /// </summary>
        /// <param name="command">Comando con los datos del participante</param>
        /// <returns>ID del participante creado</returns>
        [Authorize(Policy = Permissions.Projects.Create)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateProjectActorCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
} 