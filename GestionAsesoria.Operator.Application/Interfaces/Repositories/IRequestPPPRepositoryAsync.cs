using GestionAsesoria.Operator.Application.DTOs.RequestPPP.Response;
using GestionAsesoria.Operator.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories
{
    public interface IRequestPPPRepositoryAsync : IGenericRepositoryAsync<RequestPPP, int>
    {
        Task<List<ListRequestPPPDto>> GetAllForListAsync();
        Task<ListRequestPPPDto?> GetForListByIdAsync(int id);
        Task<RequestPPP?> GetByIdAsync(int id);
        Task UpdateAsync(RequestPPP entity);
        Task AddInternshipAsync(PreProfessionalInternship internship);
        Task<IEnumerable<StateRequestPPPByIdResponseDto>> GetStateRequestPPPByIdAsync(int id);

        // Application/Interfaces/Repositories/IRequestPPPRepositoryAsync.cs
        Task<bool> UpdateStateRequestPPPByIdAsync(int id, int newStatusId);

        //Task<string> GetEstadoByIdAsync(int id);
    }
}
