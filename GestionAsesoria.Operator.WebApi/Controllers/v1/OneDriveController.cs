using GestionAsesoria.Operator.Application.DTOs.OneDrive.Response;
using GestionAsesoria.Operator.Application.Features.OneDrive.Commands.DownloadFile;
using GestionAsesoria.Operator.Application.Features.OneDrive.Commands.UploadFile;
using GestionAsesoria.Operator.Application.Features.OneDrive.Queries.ListFile;
using GestionAsesoria.Operator.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.WebApi.Controllers.v1
{
    public class OneDriveController : BaseApiController<OneDriveController>
    {
        /// <summary>
        /// Sube un archivo a OneDrive en la carpeta Asesorados
        /// </summary>
        [Authorize(Policy = Permissions.OneDrive.Upload)]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(UploadFileCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Descarga un archivo desde OneDrive
        /// </summary>
        [Authorize(Policy = Permissions.OneDrive.Download)]
        [HttpGet("download/{fileId}")]
        public async Task<IActionResult> DownloadFile(string fileId)
        {
            var command = new DownloadFileCommand { FileId = fileId };
            var result = await _mediator.Send(command);

            return File(result.FileContent, "application/octet-stream");

        }

        /// <summary>
        /// Lista los archivos en una carpeta específica
        /// </summary>
        [Authorize(Policy = Permissions.OneDrive.View)]
        [HttpGet("files")]
        public async Task<ActionResult<ListFileResponseDto>> ListFiles(string folder = "/")
        {
            var query = new ListFilesQuery { Folder = folder };
            var result = await _mediator.Send(query);

            return Ok(result);

        }
    }
}