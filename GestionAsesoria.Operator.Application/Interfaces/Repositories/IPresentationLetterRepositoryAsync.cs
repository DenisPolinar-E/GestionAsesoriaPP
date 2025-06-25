// GestionAsesoria.Operator.Application/Interfaces/Repositories/IPresentationLetterRepositoryAsync.cs
// GestionAsesoria.Operator.Application/Interfaces/Repositories/IPresentationLetterRepositoryAsync.cs
using GestionAsesoria.Operator.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories
{
    public interface IPresentationLetterRepositoryAsync : IGenericRepositoryAsync<PresentationLetter, int>
    {
        Task<PresentationLetter> AddAsync(PresentationLetter entity, CancellationToken cancellationToken = default);
        Task<PresentationLetter> GetByRequestPPPIdAsync(int requestPPPId, CancellationToken cancellationToken = default);
    }
}