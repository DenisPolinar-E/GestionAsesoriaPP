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

        //Task<string> GetEstadoByIdAsync(int id);
    }
}
