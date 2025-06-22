using System;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class PreProfessionalInternshipRepositoryAsync : GenericRepositoryAsync<RequestPPP, int>, IPreProfessionalInternshipRepositoryAsync
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<PreProfessionalInternship> _preProfessionalInternships;
        // _mapper se elimina aquí, ya que el mapeo se hace en los handlers
        private static readonly string[] AllowedStatuses = { "En Ejecución", "Aprobado", "Finalizado" };

        public PreProfessionalInternshipRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _preProfessionalInternships = dbContext.Set<PreProfessionalInternship>();
            _context = dbContext;

        }

        public async Task<List<PreProfessionalInternshipDto>> GetAllPppAsync()
        {
            try
            {
                var entities = await _context.PreProfessionalInternship
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Student)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Company)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Representative)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.ResearchArea)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Status)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.DocumentCollection)
                    .Where(p => p.RequestPPP != null && AllowedStatuses.Contains(p.RequestPPP.Status.Value))
                    .ToListAsync();

                var dtos = entities.Select(p => new PreProfessionalInternshipDto
                {
                    Id = p.Id,
                    StudentCode = p.RequestPPP.Student.Code,
                    StudentName = p.RequestPPP?.Student != null
                    ? $"{p.RequestPPP.Student.FirstName} {p.RequestPPP.Student.SecondName}"
        :           "Sin estudiante",

                    CompanyName = p.RequestPPP?.Company?.FirstName ?? "Sin empresa",

                    Status = p.RequestPPP?.Status?.Value ?? "Sin estado",

                    StartRequest = p.RequestPPP?.StartDate
                }).ToList();


                return dtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las prácticas preprofesionales: {ex.Message}", ex);
            }
        }


        public async Task<List<FilterPreProfessionalInternshipDto>> GetFilteredPppAsync(FilterPreProfessionalInternshipDto filters)
        {
            try
            {
                var query = _context.PreProfessionalInternship
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Student)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Company)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Representative)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.ResearchArea)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Status)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.DocumentCollection)
                    .Where(p => p.RequestPPP != null && AllowedStatuses.Contains(p.RequestPPP.Status.Value))
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filters.StudentCode))
                    query = query.Where(p => p.RequestPPP.Student.Code != null && p.RequestPPP.Student.Code.Contains(filters.StudentCode));

                if (!string.IsNullOrEmpty(filters.CompanyName))
                    query = query.Where(p => p.RequestPPP.Company.FirstName != null &&
                                            p.RequestPPP.Company.FirstName.Contains(filters.CompanyName));

                if (!string.IsNullOrEmpty(filters.Status))
                    query = query.Where(p => p.RequestPPP.Status.Value == filters.Status);

                var result = await query.ToListAsync();

                var dtoList = result.Select(p => new FilterPreProfessionalInternshipDto
                {
                    StudentCode = p.RequestPPP.Student.Code,
                    CompanyName = p.RequestPPP.Company.FirstName,
                    Status = p.RequestPPP.Status.Value
                    // Agrega más propiedades si las tiene el DTO
                }).ToList();

                return dtoList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al filtrar las prácticas preprofesionales: {ex.Message}", ex);
            }
        }
    }
}