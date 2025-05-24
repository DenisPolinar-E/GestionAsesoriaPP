using GestionAsesoria.Operator.Application.DTOs.MasterDataValues;
using GestionAsesoria.Operator.Application.Features.MasterDataValues.Queries.GetSelect;
using GestionAsesoria.Operator.Application.Features.MasterDataValues.Queries.GetSelectProject;
using GestionAsesoria.Operator.Shared.Constants.Permission;
using GestionAsesoria.Operator.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.WebApi.Controllers.v1
{
    public class MasterDataValueController : BaseApiController<MasterDataValueController>
    {
        /// <summary>
        /// Get Select MasterDataValue
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = Permissions.MasterDataValues.View)]
        [HttpGet("GetSelectMasterDataValue")]
        public async Task<IActionResult> GetSelectMasterDataValue()
        {
            var response = await _mediator.Send(new GetSelectMasterDataValueQuery());
            return Ok(response);
        }

        /// <summary>
        /// Obtiene los grupos de investigación activos.
        /// </summary>
        /// <summary>
        /// Obtiene los grupos de investigación activos.
        /// </summary>
        [Authorize(Policy = Permissions.Actors.View)]
        [HttpGet("GetMehodProjectList")]
        public async Task<ActionResult<Result<IEnumerable<MasterDataValueResponseDto>>>> GetMethodProjectTypeListAsync()
        {
            var result = await _mediator.Send(new GetMehodProjectListQuery());

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene los grupos de investigación activos.
        /// </summary>
        /// <summary>
        /// Obtiene los grupos de investigación activos.
        /// </summary>
        [Authorize(Policy = Permissions.Actors.View)]
        [HttpGet("GetODSObjectiveList")]
        public async Task<ActionResult<Result<IEnumerable<MasterDataValueResponseDto>>>> GetODSObjectiveTypeListAsync()
        {
            var result = await _mediator.Send(new GetODSObjectiveListQuery());

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene la lista de clasificaciones de proyectos.
        /// </summary>
        [Authorize(Policy = Permissions.Actors.View)]
        [HttpGet("GetClassificationProjectList")]
        public async Task<ActionResult<Result<IEnumerable<MasterDataValueResponseDto>>>> GetClassificationProjectTypeListAsync()
        {
            var result = await _mediator.Send(new GetClassificationProjectListQuery());

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene la lista de tipos de financiamiento.
        /// </summary>
        [Authorize(Policy = Permissions.Actors.View)]
        [HttpGet("GetFundingTypeList")]
        public async Task<ActionResult<Result<IEnumerable<MasterDataValueResponseDto>>>> GetFundingTypeListAsync()
        {
            var result = await _mediator.Send(new GetFundingTypeListQuery());

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene la lista de tipos de autor.
        /// </summary>
        [Authorize(Policy = Permissions.Actors.View)]
        [HttpGet("GetAuthorTypeList")]
        public async Task<ActionResult<Result<IEnumerable<MasterDataValueResponseDto>>>> GetAuthorTypeListAsync()
        {
            var result = await _mediator.Send(new GetAuthorTypeListQuery());

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
