using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class PreProfessionalInternshipRepositoryAsync : IPreProfessionalInternshipRepositoryAsync
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<PreProfessionalInternship> _preProfessionalInternships;

        public PreProfessionalInternshipRepositoryAsync(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _preProfessionalInternships = dbContext.Set<PreProfessionalInternship>();
        }

        public async Task<List<PreProfessionalInternshipDto>> GetAllPppAsync()
        {
            try
            {
                var currentDate = DateTime.Now; // 06:55 PM -05 on Thursday, June 26, 2025
                var entities = await _preProfessionalInternships
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Student)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Company)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Status)
                    .Include(p => p.PreProfessionalInternshipContracts)
                        .ThenInclude(pc => pc.AdvisoringContract)
                            .ThenInclude(ac => ac.AdvisorActor)
                    .Where(p => p.RequestPPP != null)
                    .ToListAsync();

                if (!entities.Any())
                {
                    var internshipCount = await _context.PreProfessionalInternship.CountAsync();
                    var contractCount = await _context.PreProfessionalInternshipByAdvisoringContract.CountAsync();
                    var requestPPPCount = await _context.RequestPPP.CountAsync();
                    throw new Exception($"No se encontraron prácticas preprofesionales. Total PreProfessionalInternship: {internshipCount}, Total contratos: {contractCount}, Total RequestPPP: {requestPPPCount}");
                }

                var dtos = entities
                    .Select(p => new PreProfessionalInternshipDto
                    {
                        Id = p.Id,
                        StudentCode = p.RequestPPP.Student?.Code ?? "Sin código",
                        StudentName = p.RequestPPP.Student != null
                            ? $"{p.RequestPPP.Student.FirstName} {p.RequestPPP.Student.SecondName}"
                            : "Sin estudiante",
                        AdvisorName = p.PreProfessionalInternshipContracts.FirstOrDefault()?.AdvisoringContract?.AdvisorActor != null
                            ? $"{p.PreProfessionalInternshipContracts.FirstOrDefault().AdvisoringContract.AdvisorActor.FirstName} {p.PreProfessionalInternshipContracts.FirstOrDefault().AdvisoringContract.AdvisorActor.SecondName}"
                            : "Sin asesor",
                        CompanyName = p.RequestPPP.Company?.FirstName ?? "Sin empresa",
                        StartPreProfessionalPractice = p.RequestPPP.StartPreProfessionalPractice,
                        EndPreProfessionalPractice = p.RequestPPP.EndPreProfessionalPractice,
                        Status = DetermineStatus(p.RequestPPP.StartPreProfessionalPractice, p.RequestPPP.EndPreProfessionalPractice, currentDate)
                        // Note = p.Evaluation?.Note // Descomentar cuando se implemente la tabla de notas
                    }).ToList();

                if (!dtos.Any())
                {
                    var statusValues = await _context.RequestPPP
                        .Include(r => r.Status)
                        .Select(r => r.Status != null ? r.Status.Value : "Sin estado")
                        .Distinct()
                        .ToListAsync();
                    throw new Exception($"No se encontraron prácticas preprofesionales con estado válido. Valores de Status: {string.Join(", ", statusValues)}");
                }

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
                var currentDate = DateTime.Now; // 06:55 PM -05 on Thursday, June 26, 2025
                var query = _preProfessionalInternships
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Student)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Company)
                    .Include(p => p.RequestPPP)
                        .ThenInclude(r => r.Status)
                    .Include(p => p.PreProfessionalInternshipContracts)
                        .ThenInclude(pc => pc.AdvisoringContract)
                            .ThenInclude(ac => ac.AdvisorActor)
                    .Where(p => p.RequestPPP != null)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filters.StudentCode))
                    query = query.Where(p => p.RequestPPP.Student != null && p.RequestPPP.Student.Code != null && p.RequestPPP.Student.Code.Contains(filters.StudentCode));

                if (!string.IsNullOrEmpty(filters.AdvisorName))
                    query = query.Where(p => p.PreProfessionalInternshipContracts.Any(pc => pc.AdvisoringContract != null && pc.AdvisoringContract.AdvisorActor != null &&
                        EF.Functions.Like(pc.AdvisoringContract.AdvisorActor.FirstName + " " + pc.AdvisoringContract.AdvisorActor.SecondName, $"%{filters.AdvisorName}%")));

                if (!string.IsNullOrEmpty(filters.CompanyName))
                    query = query.Where(p => p.RequestPPP.Company != null && p.RequestPPP.Company.FirstName != null && p.RequestPPP.Company.FirstName.Contains(filters.CompanyName));

                if (!string.IsNullOrEmpty(filters.Status))
                    query = query.Where(p => DetermineStatus(p.RequestPPP.StartPreProfessionalPractice, p.RequestPPP.EndPreProfessionalPractice, currentDate) == filters.Status);

                var result = await query.ToListAsync();

                if (!result.Any())
                {
                    var internshipCount = await _context.PreProfessionalInternship.CountAsync();
                    var contractCount = await _context.PreProfessionalInternshipByAdvisoringContract.CountAsync();
                    var requestPPPCount = await _context.RequestPPP.CountAsync();
                    throw new Exception($"No se encontraron prácticas preprofesionales que coincidan con los filtros. Total PreProfessionalInternship: {internshipCount}, Total contratos: {contractCount}, Total RequestPPP: {requestPPPCount}");
                }

                var dtos = result.Select(p => new FilterPreProfessionalInternshipDto
                {
                    StudentCode = p.RequestPPP.Student?.Code ?? "Sin código",
                    StudentName = p.RequestPPP.Student != null
                        ? $"{p.RequestPPP.Student.FirstName} {p.RequestPPP.Student.SecondName}"
                        : "Sin estudiante",
                    AdvisorName = p.PreProfessionalInternshipContracts.FirstOrDefault()?.AdvisoringContract?.AdvisorActor != null
                        ? $"{p.PreProfessionalInternshipContracts.FirstOrDefault().AdvisoringContract.AdvisorActor.FirstName} {p.PreProfessionalInternshipContracts.FirstOrDefault().AdvisoringContract.AdvisorActor.SecondName}"
                        : "Sin asesor",
                    CompanyName = p.RequestPPP.Company?.FirstName ?? "Sin empresa",
                    StartPreProfessionalPractice = p.RequestPPP.StartPreProfessionalPractice,
                    EndPreProfessionalPractice = p.RequestPPP.EndPreProfessionalPractice,
                    Status = DetermineStatus(p.RequestPPP.StartPreProfessionalPractice, p.RequestPPP.EndPreProfessionalPractice, currentDate)
                }).ToList();

                return dtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al filtrar las prácticas preprofesionales: {ex.Message}", ex);
            }
        }

        private string DetermineStatus(DateTime? startDate, DateTime? endDate, DateTime currentDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
                return "Sin estado";

            if (currentDate < startDate)
                return "Aprobada";
            else if (currentDate >= startDate && currentDate <= endDate)
                return "En Curso";
            else
                return "Finalizada";
            // Nota: Cuando se implemente la tabla de notas, agregar lógica para "Práctica Aprobada" si la nota es >= 11.
        }
    }
}