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
    public class RoleRepositoryAsync : GenericRepositoryAsync<Role, int>, IRoleRepositoryAsync
    {
        private readonly DbSet<Role> _roles;

        public RoleRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _roles = dbContext.Set<Role>();
        }

        public async Task<int?> GetIdByNameAsync(string name)
        {
            return await _roles
                .Where(r => r.Name == name)
                .Select(r => (int?)r.Id)
                .FirstOrDefaultAsync();
        }
    }
}
