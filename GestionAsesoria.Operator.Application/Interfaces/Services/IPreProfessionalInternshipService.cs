using System.Collections.Generic;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;

namespace GestionAsesoria.Operator.Application.Interfaces.Services
{
    public interface IPreProfessionalInternshipService
    {
        Task<List<PreProfessionalInternshipDto>> GetAllAsync();
        Task<List<PreProfessionalInternshipDto>> GetFilteredAsync(FilterPreProfessionalInternshipDto filters);
    }
}