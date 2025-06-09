using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Domain.Entities.Identity;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories.Identity
{
    public interface IRoleRepositoryAsync : IGenericRepositoryAsync<Role, int>
    {
        Task<int?> GetIdByNameAsync(string name);
    }
}
