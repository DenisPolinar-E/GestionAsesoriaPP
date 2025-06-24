using GestionAsesoria.Operator.Domain.Entities.ProjectIDI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories
{
    public interface IProjectActorRepositoryAsync : IGenericRepositoryAsync<ProjectActor, int>
    {
        /// <summary>
        /// Obtiene todos los actores asociados a un proyecto
        /// </summary>
        Task<IEnumerable<ProjectActor>> GetByProjectIdAsync(int projectId);
        
        /// <summary>
        /// Verifica si un actor ya está asociado a un proyecto
        /// </summary>
        Task<bool> ActorExistsInProjectAsync(int projectId, int actorId);
    }
} 