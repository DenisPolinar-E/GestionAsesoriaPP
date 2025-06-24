using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response;

namespace GestionAsesoria.Operator.Application.Interfaces.Services
{
    public interface IActorCompanyService
    {
        Task<IEnumerable<CompanyPracticeCountDto>> GetCompaniesWithPracticeCountAsync();
    }
}

