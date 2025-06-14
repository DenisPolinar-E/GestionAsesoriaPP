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
        Task<RequestPPP?> GetByIdAsync(int id);
        Task UpdateAsync(RequestPPP entity);
        Task AddInternshipAsync(PreProfessionalInternship internship);


        //Task<string> GetEstadoByIdAsync(int id);
    }
}
