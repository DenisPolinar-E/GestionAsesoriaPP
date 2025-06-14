using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchArea;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.ConfigParameters.Container;
using GestionAsesoria.Operator.Shared.Wrapper;
using Google.Apis.Calendar.v3.Data;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tsp.Sigescom.Config;

namespace GestionAsesoria.Operator.Application.Features.Actors.Queries.GetActorsResearchAreas
{
    public class GetAllResearchAreasQuery : IRequest<Result<IEnumerable<GetAllActorResearchAreaDto>>> { }

    internal class GetAllResearchAreasQueryHandler : IRequestHandler<GetAllResearchAreasQuery, Result<IEnumerable<GetAllActorResearchAreaDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly SettingsContainer _settingsContainer;

        public GetAllResearchAreasQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _settingsContainer = LocalSettingContainer.Get();
        }

        public async Task<Result<IEnumerable<GetAllActorResearchAreaDto>>> Handle(GetAllResearchAreasQuery request, CancellationToken cancellationToken)
        {
            //var roleId = await _unitOfWork.RoleRepository.GetIdByNameAsync("Área de Investigación");
            //var roleId =  _settingsContainer.LocalResearchAreaSettings.ResearchAreaId;

            var areas = await _unitOfWork.ActorRepository.GetAllResearchAreasAsync();
            return await Result<IEnumerable<GetAllActorResearchAreaDto>>.SuccessAsync(areas);
        }
    }
}
