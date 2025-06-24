using GestionAsesoria.Operator.Application.DTOs.Generic.Response;
using GestionAsesoria.Operator.Application.DTOs.MasterDataValues;
using GestionAsesoria.Operator.Application.Features.MasterDataValues.Queries.GetSelectProject;
using GestionAsesoria.Operator.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories
{
    public interface IMasterDataValueRepositoryAsync : IGenericRepositoryAsync<MasterDataValue, int>
    {
        Task<List<GetNameAndValueDTO>> GetListMasterDataValue();
        Task<IEnumerable<GetNameAndValueDTO>> GetMasterDataValuesSelectAsync();
        Task<MasterDataValue> GetByCodeAsync(string code);
        Task<List<MasterDataValueResponseDto>> GetListProjectMasterDataValue(int MasterDataId);
        Task<List<MasterDataValueResponseDto>> GetMethodProjectTypeListAsync();
        Task<List<MasterDataValueResponseDto>> GetODSObjectiveTypeListAsync();
        Task<List<MasterDataValueResponseDto>> GetClassificationProjectTypeListAsync();
        Task<List<MasterDataValueResponseDto>> GetFundingTypeListAsync();
        Task<List<MasterDataValueResponseDto>> GetAuthorTypeListAsync();
    }
}
