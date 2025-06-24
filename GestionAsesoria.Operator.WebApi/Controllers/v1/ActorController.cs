using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchGroup;
using GestionAsesoria.Operator.Application.Features.Actors.Queries.GetActorDocentes;
using GestionAsesoria.Operator.Application.Features.Actors.Queries.GetActorResearchLines;
using GestionAsesoria.Operator.Application.Features.Actors.Queries.GetActorsByMainRole;
using GestionAsesoria.Operator.Application.Features.Actors.Queries.GetActorsResearchAreas;
using GestionAsesoria.Operator.Application.Features.Actors.Queries.GetSelect;
using GestionAsesoria.Operator.Shared.Constants.Permission;
using GestionAsesoria.Operator.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.WebApi.Controllers.v1
{
    public class ActorController : BaseApiController<ActorController>
    {
        /// <summary>
        /// Obtiene los grupos de investigación activos.
        /// </summary>
        [Authorize(Policy = Permissions.Actors.View)]
        [HttpGet("GetActorByRoleResearchGroup")]
        public async Task<ActionResult<Result<IEnumerable<GetActorResearchGroupDto>>>> GetActorByRoleResearchGroup()
        {
            var result = await _mediator.Send(new GetActorsByMainRoleQuery());

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Policy = Permissions.Actors.View)]
        [HttpGet("GetActorByRoleResearchArea")]
        public async Task<IActionResult> GetActorByRoleResearchArea([FromQuery] int? groupId = null)
        {
            var result = await _mediator.Send(new GetActorResearchAreasQuery { GroupId = groupId });
            return Ok(result);
        }

        [Authorize(Policy = Permissions.Actors.View)]
        [HttpGet("GetActorByRoleResearchLine")]
        public async Task<IActionResult> GetActorByRoleResearchLine([FromQuery] int? groupId = null)
        {
            var result = await _mediator.Send(new GetActorResearchLinesQuery { GroupId = groupId });
            return Ok(result);
        }

        [Authorize(Policy = Permissions.Actors.View)]
        [HttpGet("GetActorByRoleTeacher")]
        public async Task<IActionResult> GetActorByRoleTeacher([FromQuery] int? groupId = null)
        {
            var result = await _mediator.Send(new GetActorTeachersQuery { GroupId = groupId });
            return Ok(result);
        }




        [Authorize(Policy = Permissions.Actors.View)]
        [HttpGet("GetActorsByParent/{actorResearchGroupId}/{roleId}")]
        public async Task<ActionResult<Result<IEnumerable<ActorResponseDto>>>> GetActorsByParentAndRole(int actorResearchGroupId, int roleId)
        {
            try
            {
                // Ejecutar el Query para obtener actores por grupo y rol
                var result = await _mediator.Send(new GetChildActorsByParentAndRoleQuery
                {
                    ParentId = actorResearchGroupId,
                    RoleId = roleId
                });

                if (!result.Succeeded)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Manejo de errores y excepciones
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }


        }
    }
}