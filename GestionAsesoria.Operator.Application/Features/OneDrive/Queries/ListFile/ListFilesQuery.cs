using GestionAsesoria.Operator.Application.DTOs.OneDrive.Request;
using GestionAsesoria.Operator.Application.DTOs.OneDrive.Response;
using GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.OneDrive.Queries.ListFile
{
    public class ListFilesQuery : IRequest<ListFileResponseDto>
    {
        public string Folder { get; set; } = "/";
    }

    internal class ListFilesQueryHandler : IRequestHandler<ListFilesQuery, ListFileResponseDto>
    {
        private readonly IOneDriveService _oneDriveService;
        private const string DEFAULT_FOLDER = "/";

        public ListFilesQueryHandler(IOneDriveService oneDriveService)
        {
            _oneDriveService = oneDriveService;
        }

        public async Task<ListFileResponseDto> Handle(ListFilesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string folderPath = NormalizeFolderPath(request.Folder);
                await EnsureDefaultFolderExistsAsync();
                string folderId = await _oneDriveService.GetFolderIdAsync(folderPath);

                if (folderId == null)
                {
                    return new ListFileResponseDto
                    {
                        Items = new List<FileItemRequestDto>(),
                    };
                }

                var items = await _oneDriveService.ListItemsInFolderAsync(folderId);

                return new ListFileResponseDto
                {
                    Items = items.Select(i => new FileItemRequestDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                    }),
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string NormalizeFolderPath(string folder)
        {
            if (string.IsNullOrEmpty(folder) || folder == "/")
            {
                return DEFAULT_FOLDER;
            }

            folder = folder.TrimStart('/');

            if (!folder.StartsWith(DEFAULT_FOLDER, StringComparison.OrdinalIgnoreCase))
            {
                return Path.Combine(DEFAULT_FOLDER, folder).Replace("\\", "/");
            }

            return folder;
        }

        private async Task EnsureDefaultFolderExistsAsync()
        {
            string defaultFolderId = await _oneDriveService.GetFolderIdAsync(DEFAULT_FOLDER);

            if (defaultFolderId == null)
            {
                await _oneDriveService.CreateFolderAsync(DEFAULT_FOLDER);
            }
        }
    }
}
