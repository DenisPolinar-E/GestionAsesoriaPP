// GestionAsesoria.Operator.Application/Interfaces/Services/IPresentationLetterService.cs
using GestionAsesoria.Operator.Application.DTOs.PresentationLetter.Request;
using GestionAsesoria.Operator.Application.DTOs.PresentationLetter.Response;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Interfaces.Services
{
    public interface IPresentationLetterService
    {
        Task<int> CreatePresentationLetterAsync(CreatePresentationLetterDto dto, CancellationToken cancellationToken = default);
        Task<PresentationLetterDto> GetByRequestPPPIdAsync(int requestPPPId, CancellationToken cancellationToken = default);
    }
}