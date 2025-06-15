using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class PreProfessionalInternshipByAdvisoringContractRepositoryAsync : IPreProfessionalInternshipByAdvisoringContractRepositoryAsync
    {
        private readonly ApplicationDbContext _context;

        public PreProfessionalInternshipByAdvisoringContractRepositoryAsync(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PreProfessionalInternshipByAdvisoringContract?> GetByInternshipIdAsync(int preProfessionalInternshipId)
        {
            return await _context.PreProfessionalInternshipByAdvisoringContract
                .FirstOrDefaultAsync(x => x.PreProfessionalInternshipId == preProfessionalInternshipId);
        }


        public async Task UpdateAsync(PreProfessionalInternshipByAdvisoringContract entity)
        {
            _context.PreProfessionalInternshipByAdvisoringContract.Update(entity);
        }
    }
}
