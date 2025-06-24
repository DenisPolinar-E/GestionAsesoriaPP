using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.ConfigParameters.Container;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsp.Sigescom.Config;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class DocumentCollectionRepositoryAsync : GenericRepositoryAsync<DocumentCollection, int>, IDocumentCollectionRepositoryAsync
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<DocumentCollection> _documentCollection;
        private readonly SettingsContainer _settingsContainer;

        public DocumentCollectionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
            _documentCollection = _context.Set<DocumentCollection>();
            _settingsContainer = LocalSettingContainer.Get();
        }
    }
}
