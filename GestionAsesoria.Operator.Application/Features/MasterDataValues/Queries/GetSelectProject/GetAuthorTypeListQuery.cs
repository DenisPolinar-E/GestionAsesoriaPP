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
    public class GetAuthorTypeListQuery : IRequest<Result<IEnumerable<MasterDataValueResponseDto>>>
    {
    }

    internal class GetAuthorTypeListQueryHandler : IRequestHandler<GetAuthorTypeListQuery, Result<IEnumerable<MasterDataValueResponseDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;

        public GetAuthorTypeListQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IMessageService messageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _messageService = messageService;
        }

        public async Task<Result<IEnumerable<MasterDataValueResponseDto>>> Handle(GetAuthorTypeListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var masterdatavalues = await _unitOfWork.MasterDataValueRepository.GetAuthorTypeListAsync();

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