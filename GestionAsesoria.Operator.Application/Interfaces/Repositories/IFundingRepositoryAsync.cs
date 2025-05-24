using GestionAsesoria.Operator.Domain.Entities.ProjectIDI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories
{
    public interface IFundingRepositoryAsync : IGenericRepositoryAsync<Funding, int>
    {
        /// <summary>
        /// Obtiene todos los financiamientos asociados a un proyecto
        /// </summary>
        Task<IEnumerable<Funding>> GetByProjectIdAsync(int projectId);
        
        /// <summary>
        /// Calcula el monto total de financiamiento para un proyecto
        /// </summary>
        Task<decimal> GetTotalFundingByProjectIdAsync(int projectId);
    }
} 