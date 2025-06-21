using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.Follow.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;

namespace GestionAsesoria.Operator.Application.Features.Follow.Queries
{
    public class GetAllFollowsQuery : IRequest<Result<List<ListFollowDto>>>
    {
        public class GetAllFollowsQueryHandler : IRequestHandler<GetAllFollowsQuery, Result<List<ListFollowDto>>>
        {
            private readonly IFollowRepositoryAsync _repository;

            public GetAllFollowsQueryHandler(IFollowRepositoryAsync repository)
            {
                _repository = repository;
            }

            public async Task<Result<List<ListFollowDto>>> Handle(GetAllFollowsQuery request, CancellationToken cancellationToken)
            {
                var data = await _repository.GetAllFollowsAsync();
                return await Result<List<ListFollowDto>>.SuccessAsync(data);
            }
        }
    }
}
