using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchArea;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.Actors.Queries.GetActorsResearchAreas
{
    public class GetActorResearchAreasQuery : IRequest<Result<IEnumerable<GetActorResearchAreaDto>>>
    {
        public int? GroupId { get; set; }
    }

    internal class GetActorResearchAreasQueryHandler : IRequestHandler<GetActorResearchAreasQuery, Result<IEnumerable<GetActorResearchAreaDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;

        public GetActorResearchAreasQueryHandler(IMapper mapper,
            IUnitOfWork<int> unitOfWork,
            IEmailService emailService,
            IMessageService messageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _messageService = messageService;

        }

        public async Task<Result<IEnumerable<GetActorResearchAreaDto>>> Handle(GetActorResearchAreasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var actors = await _unitOfWork.ActorRepository.GetResearchAreasAsync(request.GroupId);
                if (!actors.Any())
                {
                    string emptyMessage = _messageService.GetDynamicMessage("General", "Validation", "QueryEmpty");
                    return await Result<IEnumerable<GetActorResearchAreaDto>>.FailAsync(emptyMessage);
                }
                string successfulMessage = _messageService.GetDynamicMessage("General", "Success", "Successful");
                return await Result<IEnumerable<GetActorResearchAreaDto>>.SuccessAsync(actors, successfulMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = _messageService.GetDynamicMessage("General", "Error", "Exception");
                return await Result<IEnumerable<GetActorResearchAreaDto>>.FailAsync(errorMessage);
            }
        }
    }
}
