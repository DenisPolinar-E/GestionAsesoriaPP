using GestionAsesoria.Operator.Application.Interfaces.Repositories.Identity;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Domain.Entities.Identity;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories.Identity
{
    public class ActorTypeRepositoryAsync : GenericRepositoryAsync<ActorType, int>, IActorTypeRepositoryAsync
    {
        private readonly DbSet<ActorType> _actorTypes;

        public ActorTypeRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _actorTypes = dbContext.Set<ActorType>();
        }

        public async Task<int?> GetIdByNameAsync(string name)
        {
            return await _actorTypes
                .Where(a => a.Name == name)
                .Select(a => (int?)a.Id)
                .FirstOrDefaultAsync();
        }
    }
}
