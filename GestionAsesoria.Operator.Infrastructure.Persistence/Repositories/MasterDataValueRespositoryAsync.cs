using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchGroup;
using GestionAsesoria.Operator.Application.DTOs.Generic.Response;
using GestionAsesoria.Operator.Application.DTOs.MasterDataValues;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GestionAsesoria.Operator.Shared.Constants.Permission.Permissions;
using Tsp.Sigescom.Config;
using GestionAsesoria.Operator.Domain.ConfigParameters.Container;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class MasterDataValueRespositoryAsync : GenericRepositoryAsync<MasterDataValue, int>, IMasterDataValueRepositoryAsync
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<MasterDataValue> _masterDataValue;
        private readonly SettingsContainer _settingsContainer;

        public MasterDataValueRespositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
            _masterDataValue = _context.Set<MasterDataValue>();
            _settingsContainer = LocalSettingContainer.Get();
        }

        public async Task<MasterDataValue> GetByCodeAsync(string code)
        {
            return await _context.MasterDataValue.FirstOrDefaultAsync(md => md.Code == code && md.IsActived);
        }

        public async Task<List<GetNameAndValueDTO>> GetListMasterDataValue()
        {
            var getAllAsync = await _masterDataValue
           .Where(x => x.IsActived == true)
           .AsNoTracking()
           .Select(x => new GetNameAndValueDTO
           {
               Id = x.Id,
               Code = x.Code,
               Name = x.Name,
               Value = x.Value
           })
           .ToListAsync();
            return getAllAsync;
        }

        public async Task<IEnumerable<GetNameAndValueDTO>> GetMasterDataValuesSelectAsync()
        {
            return await _masterDataValue
                .Select(masterDataValue => new GetNameAndValueDTO
                {
                    Id = masterDataValue.Id,
                    Code = masterDataValue.Code,
                    Name = masterDataValue.Name,
                    Value = masterDataValue.Value,
                })
                .ToListAsync();  // Convierte el resultado a lista
        }

        public async Task<List<MasterDataValueResponseDto>> GetListProjectMasterDataValue(int MasterDataId)
        {
            var getAllAsync = await _masterDataValue
           .Where(x => x.MasterDataId == MasterDataId)
           .AsNoTracking()
           .Select(x => new MasterDataValueResponseDto
           {
               Id = x.Id,
               Name = x.Name,
           })
           .ToListAsync();
            return getAllAsync;
        }
        public async Task<List<MasterDataValueResponseDto>> GetMethodProjectTypeListAsync()
        {
            var methodProjectType = _settingsContainer.LocalProjectSettings.MethodProjectTypeId;
            return await _masterDataValue
                 .Where(a => a.MasterDataId == _settingsContainer.LocalProjectSettings.MethodProjectTypeId)
                 .Select(a => new MasterDataValueResponseDto
                 {
                     Id = a.Id,
                     Name = a.Name
                 })
                 .ToListAsync();
        }
        public async Task<List<MasterDataValueResponseDto>> GetODSObjectiveTypeListAsync()
        {
            var ODSObjective = _settingsContainer.LocalProjectSettings.ODSObjectiveId;
            return await _masterDataValue
                 .Where(a => a.MasterDataId == _settingsContainer.LocalProjectSettings.ODSObjectiveId)
                 .Select(a => new MasterDataValueResponseDto
                 {
                     Id = a.Id,
                     Name = a.Name
                 })
                 .ToListAsync();
        }
        public async Task<List<MasterDataValueResponseDto>> GetClassificationProjectTypeListAsync()
        {
            var ClassificationProjectType = _settingsContainer.LocalProjectSettings.ClassificationProjectTypeId;
            return await _masterDataValue
                 .Where(a => a.MasterDataId == _settingsContainer.LocalProjectSettings.ClassificationProjectTypeId)
                 .Select(a => new MasterDataValueResponseDto
                 {
                     Id = a.Id,
                     Name = a.Name
                 })
                 .ToListAsync();
        }

        public async Task<List<MasterDataValueResponseDto>> GetFundingTypeListAsync()
        {
            var fundingType = _settingsContainer.LocalFundingSettings.FundingTypeId;
            return await _masterDataValue
                 .Where(a => a.MasterDataId == _settingsContainer.LocalFundingSettings.FundingTypeId)
                 .Select(a => new MasterDataValueResponseDto
                 {
                     Id = a.Id,
                     Name = a.Name
                 })
                 .ToListAsync();
        }

        public async Task<List<MasterDataValueResponseDto>> GetAuthorTypeListAsync()
        {
            var authorType = _settingsContainer.LocalActorProjectSettings.AuthorTypeId;
            return await _masterDataValue
                 .Where(a => a.MasterDataId == _settingsContainer.LocalActorProjectSettings.AuthorTypeId)
                 .Select(a => new MasterDataValueResponseDto
                 {
                     Id = a.Id,
                     Name = a.Name
                 })
                 .ToListAsync();
        }
    }
}
