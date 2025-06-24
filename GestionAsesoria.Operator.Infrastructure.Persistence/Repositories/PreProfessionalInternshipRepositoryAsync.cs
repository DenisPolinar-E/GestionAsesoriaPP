using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class PreProfessionalInternshipRepositoryAsync : IPreProfessionalInternshipRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;

        public PreProfessionalInternshipRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<PreProfessionalInternshipDto>> GetAllAsync()
        {
            try
            {
                var internships = await _dbContext.PreProfessionalInternship
                    .Select(p => new PreProfessionalInternshipDto
                    {
                        CodeStudentId = p.CodeStudentId ?? "",
                        FullNameInterId = p.FullNameInterId ?? "",
                        FullNameAdviserId = p.FullNameAdviserId ?? "",
                        FullNameCompanyId = p.FullNameCompanyId ?? "",
                        Career = p.Career ?? "",
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        State = p.State ?? "",
                        Note = p.Note.ToString() ?? ""
                    })
                    .ToListAsync();

                return internships ?? new List<PreProfessionalInternshipDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las prácticas preprofesionales: {ex.Message}", ex);
            }
        }

        public async Task<List<PreProfessionalInternshipDto>> GetFilteredAsync(FilterPreProfessionalInternshipDto filters)
        {
            try
            {
                var query = _dbContext.PreProfessionalInternship.AsQueryable();

                if (!string.IsNullOrEmpty(filters.CodeStudentId))
                    query = query.Where(p => p.CodeStudentId != null && p.CodeStudentId.Contains(filters.CodeStudentId));

                if (!string.IsNullOrEmpty(filters.FullNameAdviserId))
                    query = query.Where(p => p.FullNameAdviserId != null && p.FullNameAdviserId.Contains(filters.FullNameAdviserId));

                if (!string.IsNullOrEmpty(filters.FullNameCompanyId))
                    query = query.Where(p => p.FullNameCompanyId != null && p.FullNameCompanyId.Contains(filters.FullNameCompanyId));

                if (!string.IsNullOrEmpty(filters.State))
                    query = query.Where(p => p.State != null && p.State == filters.State);

                var internships = await query
                    .Select(p => new PreProfessionalInternshipDto
                    {
                        CodeStudentId = p.CodeStudentId ?? "",
                        FullNameInterId = p.FullNameInterId ?? "",
                        FullNameAdviserId = p.FullNameAdviserId ?? "",
                        FullNameCompanyId = p.FullNameCompanyId ?? "",
                        Career = p.Career ?? "",
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        State = p.State ?? "",
                        Note = p.Note.ToString() ?? ""
                    })
                    .ToListAsync();

                return internships ?? new List<PreProfessionalInternshipDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al filtrar las prácticas preprofesionales: {ex.Message}", ex);
            }
        }
    }
}