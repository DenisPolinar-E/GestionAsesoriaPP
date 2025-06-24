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
    public class ProjectActorRepositoryAsync : GenericRepositoryAsync<ProjectActor, int>, IProjectActorRepositoryAsync
    {
        private readonly DbSet<ProjectActor> _projectActors;

        public ProjectActorRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _projectActors = dbContext.Set<ProjectActor>();
        }

        public async Task<IEnumerable<ProjectActor>> GetByProjectIdAsync(int projectId)
        {
            return await _projectActors
                .Where(pa => pa.ProjectId == projectId)
                .Include(pa => pa.Actor)
                .Include(pa => pa.AuthorType)
                .ToListAsync();
        }

        public async Task<bool> ActorExistsInProjectAsync(int projectId, int actorId)
        {
            return await _projectActors
                .AnyAsync(pa => pa.ProjectId == projectId && pa.ActorId == actorId);
        }
    }
} 