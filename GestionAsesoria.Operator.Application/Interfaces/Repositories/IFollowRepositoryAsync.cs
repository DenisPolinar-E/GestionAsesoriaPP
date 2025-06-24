using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.Follow.Request;
using GestionAsesoria.Operator.Application.DTOs.Follow.Response;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories
{
    public interface IFollowRepositoryAsync
    {
        Task<List<FollowDto>> GetAllFollowsAsync();
        Task<List<FollowDto>>GetProgresPppAsync();
        Task<List<FollowDto>> GetFilteredAsync(FilterFollowDto filters);

    }
}
