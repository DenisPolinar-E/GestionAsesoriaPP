using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.Follow.Request;
using GestionAsesoria.Operator.Application.DTOs.Follow.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;

namespace GestionAsesoria.Operator.Application.Features.Follow.Queries
{
    public class GetFilteredFollowsQuery : IRequest<Result<List<ListFollowDto>>>
    {
        public FollowFilterDto Filters { get; set; }

        public GetFilteredFollowsQuery(FollowFilterDto filters)
        {
            Filters = filters ?? throw new ArgumentNullException(nameof(filters));
        }
    }

}
