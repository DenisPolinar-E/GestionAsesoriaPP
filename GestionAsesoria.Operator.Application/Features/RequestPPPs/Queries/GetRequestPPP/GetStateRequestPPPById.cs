using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.RequestPPP.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.RequestPPPs.Queries.GetRequestPPP
{
    public class GetStateRequestPPPByIdQuery: IRequest<Result<IEnumerable<StateRequestPPPByIdResponseDto>>>
    {
        public int RequestPPPId { get; set; }

        public GetStateRequestPPPByIdQuery(int requestPPPId)
        {
            RequestPPPId = requestPPPId;
        }

        internal  class GetStateRequestPPPQueryHandler : IRequestHandler<GetStateRequestPPPByIdQuery, Result<IEnumerable<StateRequestPPPByIdResponseDto>>>
        {
            public readonly IUnitOfWork<int> _unitOfWork;
            public readonly IMessageService _messageService;
            public readonly IMapper _mapper;

            public GetStateRequestPPPQueryHandler(IUnitOfWork<int> unitOfWork, IMessageService messageService, IMapper mapper)
            {
                _unitOfWork=unitOfWork;
                _messageService = messageService;
                _mapper=mapper;
            }

            public async Task<Result<IEnumerable<StateRequestPPPByIdResponseDto>>> Handle(GetStateRequestPPPByIdQuery request,CancellationToken cancellation)
            {
                var estado = await _unitOfWork.RequestPPPRepository.GetStateRequestPPPByIdAsync(request.RequestPPPId);
                if (estado == null || !estado.Any())
                    return await Result<IEnumerable<StateRequestPPPByIdResponseDto>>.FailAsync("Solicitud no encontrada");

                return await Result<IEnumerable<StateRequestPPPByIdResponseDto>>.SuccessAsync(estado, "Solicitud encontrada correctamente.");
            }
        }


    }
}
