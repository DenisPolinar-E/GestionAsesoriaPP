using System.Collections.Generic;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorCompany;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories
{
    public interface ICompanyPracticeRepositoryAsync
    {
        Task<IEnumerable<ActorCompanyDto>> GetCompanyPracticeCountsAsync();
    }
}
