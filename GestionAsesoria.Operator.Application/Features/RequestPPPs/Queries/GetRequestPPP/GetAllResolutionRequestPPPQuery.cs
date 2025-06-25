using GestionAsesoria.Operator.Application.DTOs.RequestPPP.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.RequestPPPs.Queries.GetRequestPPP
{
    public class GetAllResolutionRequestPPPQuery: IRequest<List<ListResolutionRequestPPPDto>>
    {
       
        public class Handler : IRequestHandler<GetAllResolutionRequestPPPQuery, List<ListResolutionRequestPPPDto>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            public Handler(IUnitOfWork<int> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<ListResolutionRequestPPPDto>> Handle(GetAllResolutionRequestPPPQuery request, CancellationToken cancellationToken)
            {
                return await _unitOfWork.RequestPPPRepository.GetAllForResolutionListAsync();
            }
        }
        
    }
}
