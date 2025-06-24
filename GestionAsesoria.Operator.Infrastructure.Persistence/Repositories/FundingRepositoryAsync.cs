using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities.ProjectIDI;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class FundingRepositoryAsync : GenericRepositoryAsync<Funding, int>, IFundingRepositoryAsync
    {
        private readonly DbSet<Funding> _fundings;

        public FundingRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _fundings = dbContext.Set<Funding>();
        }

        public async Task<IEnumerable<Funding>> GetByProjectIdAsync(int projectId)
        {
            return await _fundings
                .Where(f => f.ProjectId == projectId)
                .Include(f => f.Project)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalFundingByProjectIdAsync(int projectId)
        {
            return await _fundings
                .Where(f => f.ProjectId == projectId)
                .SumAsync(f => f.Amount);
        }
    }
} 