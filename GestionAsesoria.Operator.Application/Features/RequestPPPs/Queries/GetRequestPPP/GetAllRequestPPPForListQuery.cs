using GestionAsesoria.Operator.Application.DTOs.RequestPPP.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.RequestPPPs.Queries
{
    public class GetAllRequestPPPForListQuery : IRequest<List<ListRequestPPPDto>>
    {
        public class Handler : IRequestHandler<GetAllRequestPPPForListQuery, List<ListRequestPPPDto>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;

            public Handler(IUnitOfWork<int> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<ListRequestPPPDto>> Handle(GetAllRequestPPPForListQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.RequestPPPRepository.GetAllForListAsync();
                //MANEJO DE EXCEPCIONES PARA SABER SI NO HAY SOLICITUDES REGISTRADAS
                if (result == null || result.Count == 0)
                {
                    throw new KeyNotFoundException("No existen solicitudes de prácticas registradas.");
                }

                return result;
            }

        }
    }
}
