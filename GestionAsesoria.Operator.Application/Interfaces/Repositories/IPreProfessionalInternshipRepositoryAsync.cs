using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories
{
    public interface IPreProfessionalInternshipRepositoryAsync
    {
        Task<List<PreProfessionalInternshipDto>> GetAllPppAsync();
        Task<List<FilterPreProfessionalInternshipDto>> GetFilteredPppAsync(FilterPreProfessionalInternshipDto filters);

    }
}
