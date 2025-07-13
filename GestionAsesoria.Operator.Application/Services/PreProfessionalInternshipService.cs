using System.Collections.Generic;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;

namespace GestionAsesoria.Operator.Application.Services
{
    public class PreProfessionalInternshipService : IPreProfessionalInternshipService
    {
        private readonly IPreProfessionalInternshipRepositoryAsync _repository;

        public PreProfessionalInternshipService(IPreProfessionalInternshipRepositoryAsync repository)
        {
            _repository = repository;
        }

        public async Task<List<PreProfessionalInternshipDto>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return list;
        }

        public async Task<List<PreProfessionalInternshipDto>> GetFilteredAsync(FilterPreProfessionalInternshipDto filters)
        {
            var list = await _repository.GetFilteredAsync(filters);
            return list;
        }
    }
}