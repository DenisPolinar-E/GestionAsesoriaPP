using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest
{
    public interface IOneDriveService
    {
        Task<string> CreateFolderAsync(string folderName, string parentFolderPath = null);
        Task<string> GetFolderIdAsync(string folderPath);
        Task<string> UploadFileToFolderAsync(string fileName, byte[] content);
        Task<byte[]> DownloadFileAsync(string fileId);
        Task<IEnumerable<(string Name, string Id, bool IsFolder)>> ListItemsInFolderAsync(string folderId);
    }
}
