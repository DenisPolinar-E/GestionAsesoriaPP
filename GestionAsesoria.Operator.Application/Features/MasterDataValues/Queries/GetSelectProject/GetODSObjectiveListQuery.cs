using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.MasterDataValues;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.MasterDataValues.Queries.GetSelectProject
{
    public class GetODSObjectiveListQuery : IRequest<Result<IEnumerable<MasterDataValueResponseDto>>>
    {
    }

    internal class GetODSObjectiveListQueryHandler : IRequestHandler<GetODSObjectiveListQuery, Result<IEnumerable<MasterDataValueResponseDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;

        public GetODSObjectiveListQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IMessageService messageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _messageService = messageService;
        }

        public async Task<Result<IEnumerable<MasterDataValueResponseDto>>> Handle(GetODSObjectiveListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var masterdatavalues = await _unitOfWork.MasterDataValueRepository.GetODSObjectiveTypeListAsync();

                if (!masterdatavalues.Any())
                {
                    string emptyMessage = _messageService.GetDynamicMessage("General", "Validation", "QueryEmpty");
                    return await Result<IEnumerable<MasterDataValueResponseDto>>.FailAsync(emptyMessage);
                }

                string successfulMessage = _messageService.GetDynamicMessage("General", "Success", "Successful");
                return await Result<IEnumerable<MasterDataValueResponseDto>>.SuccessAsync(masterdatavalues, successfulMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = _messageService.GetDynamicMessage("General", "Error", "Exception");
                // Aquí podrías agregar logs con ex.Message
                return await Result<IEnumerable<MasterDataValueResponseDto>>.FailAsync(errorMessage);
            }
        }
    }
}
