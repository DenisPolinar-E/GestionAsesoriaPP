using GestionAsesoria.Operator.Domain.Entities.Identity;
using GestionAsesoria.Operator.Domain.Entities;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories.Identity
{
    public interface IActorTypeRepositoryAsync : IGenericRepositoryAsync<ActorType, int>
    {
        Task<int?> GetIdByNameAsync(string name);
    }
}
