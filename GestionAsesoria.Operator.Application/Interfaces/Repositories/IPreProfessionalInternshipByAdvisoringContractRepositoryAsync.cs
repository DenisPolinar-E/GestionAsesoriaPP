using GestionAsesoria.Operator.Domain.Entities;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories
{
    public interface IPreProfessionalInternshipByAdvisoringContractRepositoryAsync
    {
        Task<PreProfessionalInternshipByAdvisoringContract?> GetByInternshipIdAsync(int preProfessionalInternshipId);

        Task UpdateAsync(PreProfessionalInternshipByAdvisoringContract entity);
    }
}
