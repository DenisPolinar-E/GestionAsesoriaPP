using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorCompany;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Features.Companies.Queries.GetCompanyPracticeCounts
{
    internal class GetAllCompanyPracticeCountsQueryHandler : IRequestHandler<GetAllCompanyPracticeCountsQuery, Result<IEnumerable<ActorCompanyDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllCompanyPracticeCountsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<ActorCompanyDto>>> Handle(GetAllCompanyPracticeCountsQuery request, CancellationToken cancellationToken)
        {
            var companies = await _unitOfWork.CompanyPracticeRepository.GetCompanyPracticeCountsAsync();
            return await Result<IEnumerable<ActorCompanyDto>>.SuccessAsync(companies);
        }
    }
}
