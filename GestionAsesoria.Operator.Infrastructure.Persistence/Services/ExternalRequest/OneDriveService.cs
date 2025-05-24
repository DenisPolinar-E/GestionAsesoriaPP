using Azure.Identity;
using GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Services.ExternalRequest
{
    public class OneDriveService : IOneDriveService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly IConfiguration _configuration;
        private string _driveId;

        public OneDriveService(IConfiguration configuration, ILogger<OneDriveService> logger)
        {
            _configuration = configuration;
            _graphClient = InitializeGraphServiceClient();
        }

        private GraphServiceClient InitializeGraphServiceClient()
        {
            try
            {
                var clientId = _configuration["AzureAd:ClientId"];
                var clientSecret = _configuration["AzureAd:ClientSecret"];
                var tenantId = _configuration["AzureAd:TenantId"];

                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(tenantId))
                {
                    throw new Exception("Las credenciales de Azure AD no están configuradas correctamente");
                }

                var options = new TokenCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
                };

                var clientSecretCredential = new ClientSecretCredential(
                    tenantId, clientId, clientSecret, options);

                return new GraphServiceClient(clientSecretCredential, new[] { "https://graph.microsoft.com/.default" });
            }
            catch (Exception ex)
            {
                throw new Exception("Error al inicializar el cliente de Microsoft Graph", ex);
            }
        }

        private async Task<string> GetDriveIdAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(_driveId))
                {
                    return _driveId;
                }

                var drives = await _graphClient.Drives.GetAsync();

                if (drives?.Value == null || drives.Value.Count == 0)
                {
                    throw new Exception("No se encontraron drives disponibles en la cuenta");
                }

                _driveId = drives.Value[0].Id;

                if (string.IsNullOrEmpty(_driveId))
                {
                    throw new Exception("No se pudo obtener el ID del drive de OneDrive");
                }

                return _driveId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener DriveId: {ex.Message}", ex);
            }
        }

        // Implementación de métodos de la interfaz
        public async Task<string> CreateFolderAsync(string folderName, string parentFolderPath = null)
        {
            return await GetDriveIdAsync();
        }

        public async Task<string> GetFolderIdAsync(string folderPath)
        {
            return await GetDriveIdAsync();
        }

        public async Task<string> UploadFileToFolderAsync(string fileName, byte[] content)
        {
            try
            {
                var driveId = await GetDriveIdAsync();

                using var stream = new MemoryStream(content);

                var response = await _graphClient.Drives[driveId].Root
                    .ItemWithPath(fileName)
                    .Content
                    .PutAsync(stream);

                if (response == null)
                {
                    throw new Exception("No se pudo subir el archivo");
                }

                return response.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al subir el archivo a OneDrive: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> DownloadFileAsync(string fileId)
        {
            try
            {
                var driveId = await GetDriveIdAsync();

                var stream = await _graphClient.Drives[driveId].Items[fileId].Content
                    .GetAsync();

                if (stream == null)
                {
                    throw new Exception("No se pudo obtener el contenido del archivo");
                }

                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al descargar el archivo de OneDrive: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<(string Name, string Id, bool IsFolder)>> ListItemsInFolderAsync(string folderId)
        {
            try
            {
                var driveId = await GetDriveIdAsync();

                var items = await _graphClient.Drives[driveId].Root.GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Expand = new string[] { "children" };
                });

                var result = new List<(string Name, string Id, bool IsFolder)>();

                if (items?.Children != null)
                {
                    foreach (var item in items.Children)
                    {
                        if (item.Name != null)
                        {
                            result.Add((item.Name, item.Id, false));
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar elementos: {ex.Message}", ex);
            }
        }
    }
}