using GestionAsesoria.Operator.Application.DTOs.OneDrive.Response;
using GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.OneDrive.Commands.DownloadFile
{
    public class DownloadFileCommand : IRequest<DownloadFileResponseDto>
    {
        [Required]
        public string FileId { get; set; }
    }

    internal class DownloadFileCommandHandler : IRequestHandler<DownloadFileCommand, DownloadFileResponseDto>
    {
        private readonly IOneDriveService _oneDriveService;
        private readonly ILogger<DownloadFileCommandHandler> _logger;

        public DownloadFileCommandHandler(IOneDriveService oneDriveService, ILogger<DownloadFileCommandHandler> logger)
        {
            _oneDriveService = oneDriveService;
            _logger = logger;
        }

        public async Task<DownloadFileResponseDto> Handle(DownloadFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var fileContent = await _oneDriveService.DownloadFileAsync(request.FileId);

                return new DownloadFileResponseDto
                {
                    FileContent = fileContent
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el handler de DownloadFile: {Message}", ex.Message);
                throw;
            }
        }
    }
}
