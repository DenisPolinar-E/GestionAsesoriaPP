using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class PresentationLetterRepositoryAsync
        : GenericRepositoryAsync<PresentationLetter, int>, IPresentationLetterRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;

        public PresentationLetterRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        // Método adicional específico de esta clase
        public async Task<PresentationLetter> GetByRequestPPPIdAsync(int requestPPPId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<PresentationLetter>()
                .FirstOrDefaultAsync(x => x.RequestPPPId == requestPPPId, cancellationToken);
        }

        // Sobrecarga del AddAsync si es necesario
        public new async Task<PresentationLetter> AddAsync(PresentationLetter entity, CancellationToken cancellationToken = default)
        {
            await base.AddAsync(entity); // Usa el método base
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}
