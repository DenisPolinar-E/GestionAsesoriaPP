using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorCompany;
using GestionAsesoria.Operator.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;

namespace GestionAsesoria.Operator.Application.Features.Companies.Queries.GetCompanyPracticeCounts
{
    public class GetAllCompanyPracticeCountsQuery : IRequest<Result<IEnumerable<ActorCompanyDto>>>
    {
    }
}
