using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorTeacher;
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
using Tsp.Sigescom.Config;

namespace GestionAsesoria.Operator.Application.Features.Actors.Queries.GetActorDocentes
{
    public class GetActorTeachersQuery : IRequest<Result<IEnumerable<GetActorTeacherDto>>>
    {
        public int? GroupId { get; set; }
    }

    internal class GetActorTeachersQueryHandler : IRequestHandler<GetActorTeachersQuery, Result<IEnumerable<GetActorTeacherDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;

        //Parámetros
        private readonly SettingsContainer _settingsContainer;

        public GetActorTeachersQueryHandler(IMapper mapper,
            IUnitOfWork<int> unitOfWork,
            IEmailService emailService,
            IMessageService messageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _messageService = messageService;

        }

        public async Task<Result<IEnumerable<GetActorTeacherDto>>> Handle(GetActorTeachersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var actors = await _unitOfWork.ActorRepository.GetTeachersAsync(request.GroupId);

                if (!actors.Any())
                {
                    string emptyMessage = _messageService.GetDynamicMessage("General", "Validation", "QueryEmpty");
                    return await Result<IEnumerable<GetActorTeacherDto>>.FailAsync(emptyMessage);
                }

                string successfulMessage = _messageService.GetDynamicMessage("General", "Success", "Successful");
                return await Result<IEnumerable<GetActorTeacherDto>>.SuccessAsync(actors, successfulMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = _messageService.GetDynamicMessage("General", "Error", "Exception");
                return await Result<IEnumerable<GetActorTeacherDto>>.FailAsync(errorMessage);
            }
        }
    }
}
