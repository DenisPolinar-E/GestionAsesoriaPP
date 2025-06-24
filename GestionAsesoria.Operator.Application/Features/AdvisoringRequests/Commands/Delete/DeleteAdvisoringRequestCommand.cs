using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using GestionAsesoria.Operator.Shared.Static;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.AdvisoringRequests.Commands.Delete
{
    public class DeleteAdvisoringRequestCommand : IRequest<Result<int>>
    {
        public int AdvisoringRequestId { get; set; }
    }

    internal class DeleteAdvisoringRequestCommandHandler : IRequestHandler<DeleteAdvisoringRequestCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly ILogger<DeleteAdvisoringRequestCommandHandler> _logger;

        public DeleteAdvisoringRequestCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IMessageService messageService,
            ILogger<DeleteAdvisoringRequestCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(DeleteAdvisoringRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var advisoringRequest = await _unitOfWork.AdvisoringRequestRepository.GetByIdAsync(request.AdvisoringRequestId);

                if (advisoringRequest is null)
                {
                    string notFoundMessage = _messageService.GetDynamicMessage("AdvisoringRequest", "Messages", "NotFound");
                    return await Result<int>.FailAsync(notFoundMessage);
                }

                await _unitOfWork.AdvisoringRequestRepository.DeleteAsync(advisoringRequest);
                await _unitOfWork.Commit(cancellationToken);

                string successMessage = _messageService.GetDynamicMessage("AdvisoringRequest", "Messages", "Delete_Success");
                return await Result<int>.SuccessAsync(advisoringRequest.Id, successMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la solicitud de asesoría");
                string errorMessage = _messageService.GetDynamicMessage("General", "Error", "Exception");
                return await Result<int>.FailAsync(errorMessage);
            }
        }
    }
}
