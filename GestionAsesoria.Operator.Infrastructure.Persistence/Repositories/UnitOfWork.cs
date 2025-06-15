using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Repositories.Identity;
using GestionAsesoria.Operator.Application.Interfaces.Repositories.Projects;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest;
using GestionAsesoria.Operator.Domain.Auditable;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repositories;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repositories.Identity;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repositories.Projects;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repository;
using LazyCache;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Infrastructure.Repositories
{
    public class UnitOfWork<TId> : IUnitOfWork<TId>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ApplicationDbContext _dbContext;
        private bool disposed;
        private Hashtable _repositories;
        private readonly IAppCache _cache;


        public UnitOfWork(ApplicationDbContext dbContext, ICurrentUserService currentUserService, IAppCache cache)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _currentUserService = currentUserService;
            _cache = cache;
        }

        public IGenericRepositoryAsync<TEntity, TId> Repository<TEntity>() where TEntity : AuditableEntity<TId>
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepositoryAsync<,>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity), typeof(TId)), _dbContext);
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepositoryAsync<TEntity, TId>)_repositories[type]!;
        }

        public IEmailService EmailService => null!;
        public IMasterDataValueRepositoryAsync MasterDataValueRepository => _masterDataValue ?? new MasterDataValueRespositoryAsync(_dbContext);
        public IActorRepositoryAsync ActorRepository => _actor ?? new ActorRepositoryAsync(_dbContext);
        public IAdvisoringContractRepositoryAsync AdvisoringContractRepository => _AdvisoringContract ?? new AdvisoringContractsRepositoryAsync(_dbContext);
        public IAdvisoringRequestRepositoryAsync AdvisoringRequestRepository => _advisoringRequestRepository ?? new AdvisoringRequestRepositoryAsync(_dbContext);
        public IProjectRepositoryAsync ProjectRepository => _projectRepository ?? new ProjectRepositoryAsync(_dbContext);
        public IDocumentCollectionRepositoryAsync DocumentCollectionRepository => _documentCollectionRepository ?? new DocumentCollectionRepositoryAsync(_dbContext);
        public IProjectActorRepositoryAsync ProjectActorRepository => _projectActorRepository ?? new ProjectActorRepositoryAsync(_dbContext);
        public IFundingRepositoryAsync FundingRepository => _fundingRepository ?? new FundingRepositoryAsync(_dbContext);
        public IRequestPPPRepositoryAsync RequestPPPRepository => _requestPPPRepository ?? new RequestPPPRepositoryAsync(_dbContext);
        public IRoleRepositoryAsync RoleRepository => _roleRepository ?? new RoleRepositoryAsync(_dbContext);
        public IActorTypeRepositoryAsync ActorTypeRepository => _actorTypeRepository ?? new ActorTypeRepositoryAsync(_dbContext);
        public ICompanyPracticeRepositoryAsync CompanyPracticeRepository => _companyPracticeRepository ?? new CompanyPracticeRepository(_dbContext);
        public IPreProfessionalInternshipByAdvisoringContractRepositoryAsync PreProfessionalInternshipByAdvisoringContractRepository => _internshipContractRepository ?? new PreProfessionalInternshipByAdvisoringContractRepositoryAsync(_dbContext);



        private IMasterDataValueRepositoryAsync _masterDataValue => null!;
        private IActorRepositoryAsync _actor => null!;
        private IAdvisoringContractRepositoryAsync _AdvisoringContract => null!;
        private IAdvisoringRequestRepositoryAsync _advisoringRequestRepository => null!;
        private IProjectRepositoryAsync _projectRepository => null!;
        private IDocumentCollectionRepositoryAsync _documentCollectionRepository => null!;
        private IProjectActorRepositoryAsync _projectActorRepository => null!;
        private IFundingRepositoryAsync _fundingRepository => null!;
        private IRequestPPPRepositoryAsync _requestPPPRepository => null!;
        private ICompanyPracticeRepositoryAsync _companyPracticeRepository => null!;
        private IRoleRepositoryAsync _roleRepository => null;
        private IActorTypeRepositoryAsync _actorTypeRepository => null!;
        private IPreProfessionalInternshipByAdvisoringContractRepositoryAsync _internshipContractRepository => null!;

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys)
        {
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            foreach (var cacheKey in cacheKeys)
            {
                _cache.Remove(cacheKey);
            }
            return result;
        }

        public IDbTransaction BeginTransaction()
        {
            var transaction = _dbContext.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        }

        public async Task<T> ExecuteInTransactionAsync<T>(
            Func<Task<T>> operation,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
                try
                {
                    var result = await operation();
                    await transaction.CommitAsync(cancellationToken);
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        public async Task Rollback()
        {
            _dbContext.ChangeTracker.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }
    }
}