using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchArea;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.Actors.Queries.GetActorsResearchAreas
{
    public class GetAllResearchAreasQuery : IRequest<Result<IEnumerable<GetAllActorResearchAreaDto>>> { }

    internal class GetAllResearchAreasQueryHandler : IRequestHandler<GetAllResearchAreasQuery, Result<IEnumerable<GetAllActorResearchAreaDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllResearchAreasQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<GetAllActorResearchAreaDto>>> Handle(GetAllResearchAreasQuery request, CancellationToken cancellationToken)
        {
            var roleId = await _unitOfWork.RoleRepository.GetIdByNameAsync("Área de Investigación");

            var areas = await _unitOfWork.ActorRepository.GetAllResearchAreasAsync(roleId);
            return await Result<IEnumerable<GetAllActorResearchAreaDto>>.SuccessAsync(areas);
        }
    }
}
