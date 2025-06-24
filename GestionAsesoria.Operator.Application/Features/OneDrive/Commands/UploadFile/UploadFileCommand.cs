using GestionAsesoria.Operator.Application.DTOs.OneDrive.Response;
using GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.OneDrive.Commands.UploadFile
{
    public class UploadFileCommand : IRequest<UploadFileResponseDto>
    {
        [Required(ErrorMessage = "El archivo es obligatorio")]
        public IFormFile File { get; set; }
    }

    internal class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, UploadFileResponseDto>
    {
        private readonly IOneDriveService _oneDriveService;
        private const string DEFAULT_FOLDER = "Asesorados";

        public UploadFileCommandHandler(IOneDriveService oneDriveService)
        {
            _oneDriveService = oneDriveService;
        }

        public async Task<UploadFileResponseDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Leer el contenido del archivo
                using var memoryStream = new MemoryStream();
                await request.File.CopyToAsync(memoryStream, cancellationToken);
                memoryStream.Position = 0;
                var fileBytes = memoryStream.ToArray();

                string fileId = await _oneDriveService.UploadFileToFolderAsync(request.File.FileName, fileBytes);

                return new UploadFileResponseDto
                {
                    Message = "Archivo subido exitosamente",
                    FileName = request.File.FileName,
                    FileId = fileId,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string> EnsureFolderStructureExistsAsync(string folder)
        {
            try
            {
                string asesoradosFolderId = await _oneDriveService.GetFolderIdAsync(DEFAULT_FOLDER);

                if (asesoradosFolderId == null)
                {
                    try
                    {
                        await _oneDriveService.CreateFolderAsync(DEFAULT_FOLDER);
                        asesoradosFolderId = await _oneDriveService.GetFolderIdAsync(DEFAULT_FOLDER);
                        if (asesoradosFolderId == null)
                        {
                            throw new Exception("No se pudo crear la carpeta base Asesorados");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error al crear la carpeta base Asesorados: {ex.Message}");
                    }
                }

                if (string.IsNullOrEmpty(folder) || folder == "/")
                {
                    return DEFAULT_FOLDER;
                }

                // Preparar la ruta completa
                folder = folder.TrimStart('/');
                string fullPath = Path.Combine(DEFAULT_FOLDER, folder).Replace("\\", "/");

                try
                {
                    string fullPathId = await _oneDriveService.GetFolderIdAsync(fullPath);

                    if (fullPathId != null)
                    {
                        return fullPath;
                    }

                    string[] pathParts = folder.Split('/', StringSplitOptions.RemoveEmptyEntries);
                    string currentPath = DEFAULT_FOLDER;

                    foreach (var part in pathParts)
                    {
                        string nextPath = $"{currentPath}/{part}";
                        string nextPathId = await _oneDriveService.GetFolderIdAsync(nextPath);

                        if (nextPathId == null)
                        {
                            try
                            {
                                await _oneDriveService.CreateFolderAsync(part, currentPath);
                                nextPathId = await _oneDriveService.GetFolderIdAsync(nextPath);
                                if (nextPathId == null)
                                {
                                    throw new Exception($"No se pudo crear la carpeta: {nextPath}");
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Error al crear la carpeta {nextPath}: {ex.Message}");
                            }
                        }

                        currentPath = nextPath;
                    }

                    return fullPath;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al crear la estructura de carpetas para {fullPath}: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al asegurar la estructura de carpetas: {ex.Message}", ex);
            }
        }
    }
}
