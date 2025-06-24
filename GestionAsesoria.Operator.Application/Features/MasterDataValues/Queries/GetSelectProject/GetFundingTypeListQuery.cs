using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.MasterDataValues;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.MasterDataValues.Queries.GetSelectProject
{
    public class GetFundingTypeListQuery : IRequest<Result<IEnumerable<MasterDataValueResponseDto>>>
    {
    }

    internal class GetFundingTypeListQueryHandler : IRequestHandler<GetFundingTypeListQuery, Result<IEnumerable<MasterDataValueResponseDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;

        public GetFundingTypeListQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IMessageService messageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _messageService = messageService;
        }

        public async Task<Result<IEnumerable<MasterDataValueResponseDto>>> Handle(GetFundingTypeListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var masterdatavalues = await _unitOfWork.MasterDataValueRepository.GetFundingTypeListAsync();

                if (!masterdatavalues.Any())
                {
                    string emptyMessage = _messageService.GetDynamicMessage("General", "Validation", "QueryEmpty");
                    return await Result<IEnumerable<MasterDataValueResponseDto>>.FailAsync(emptyMessage);
                }

                return await Result<IEnumerable<MasterDataValueResponseDto>>.SuccessAsync(masterdatavalues);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<MasterDataValueResponseDto>>.FailAsync(ex.Message);
            }
        }
    }
} 