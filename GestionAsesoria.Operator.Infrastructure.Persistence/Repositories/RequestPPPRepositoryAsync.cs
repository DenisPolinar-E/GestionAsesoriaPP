using GestionAsesoria.Operator.Application.DTOs.RequestPPP.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Domain.Entities.ProjectIDI;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class RequestPPPRepositoryAsync : GenericRepositoryAsync<RequestPPP, int>, IRequestPPPRepositoryAsync
    {
        private readonly DbSet<RequestPPP> _requestPPP;
        private readonly DbSet<Actor> _actor;
        private readonly ApplicationDbContext _context;

        public RequestPPPRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _requestPPP= dbContext.Set<RequestPPP>();
            _actor= dbContext.Set<Actor>();
            _context = dbContext;
        }
        public async Task<List<ListRequestPPPDto>> GetAllForListAsync()
        {
            return await _requestPPP
                .AsNoTracking()
                .Include(r => r.Student)
                .Include(r => r.Company)
                .Include(r => r.Representative)
                .Include(r => r.ResearchArea)
                .Include(r => r.Status)
                .Include(r => r.DocumentCollection)
                .Select(r => new ListRequestPPPDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Modality = r.Modality,
                    Status = r.Status.Name,

                    StartDate = r.StartDate,
                    StartPreProfessionalPractice = r.StartPreProfessionalPractice,
                    EndPreProfessionalPractice = r.EndPreProfessionalPractice,

                    StudentName = r.Student.FirstName + r.Student.SecondName,
                    CompanyName = r.Company.FirstName,
                    AcademicAreaName = r.ResearchArea.FirstName,

                    DocumentUrl = r.DocumentCollection.OnlineUrl
                })
                .ToListAsync();
        }
        public async Task<ListRequestPPPDto?> GetForListByIdAsync(int id)
        {
            return await _requestPPP
                .AsNoTracking()
                .Include(r => r.Student)
                .Include(r => r.Company)
                .Include(r => r.Representative)
                .Include(r => r.ResearchArea)
                .Include(r => r.Status)
                .Include(r => r.DocumentCollection)
                .Where(r => r.Id == id)
                .Select(r => new ListRequestPPPDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Modality = r.Modality,
                    Status = r.Status.Name,
                    StartDate = r.StartDate,
                    StudentName = r.Student.FirstName + r.Student.SecondName,
                    CompanyName = r.Company.FirstName,
                    AcademicAreaName = r.ResearchArea.FirstName,
                    DocumentUrl = r.DocumentCollection.OnlineUrl
                })
                .FirstOrDefaultAsync();
        }

        public async Task<RequestPPP?> GetByIdAsync(int id)
        {
            return await _requestPPP
                .Include(x => x.Student)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(RequestPPP entity)
        {
            _context.RequestPPP.Update(entity);
        }
        public async Task AddInternshipAsync(PreProfessionalInternship internship)
        {
            await _context.PreProfessionalInternship.AddAsync(internship);
        }

        public async Task<IEnumerable<StateRequestPPPByIdResponseDto>> GetStateRequestPPPByIdAsync(int id)
        {
            var estado = await _requestPPP
                .Where(x => x.Id == id)
                .Select(x => new StateRequestPPPByIdResponseDto
                {
                    State = x.Status.Value
                })
                .ToListAsync();
            return estado;
        }
        public async Task<bool> UpdateStateRequestPPPByIdAsync(int id, int newStatusId)
        {
            var entity = await _requestPPP.FindAsync(id);
            if (entity == null)
                return false;

            entity.StatusId = newStatusId;
            // si necesitas cargar la navegación:
            // entity.Status = await _context.MasterDataValue.FindAsync(newStatusId);

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<ListResolutionRequestPPPDto>> GetAllForResolutionListAsync()
        {
                var query =
               from r in _context.RequestPPP.AsNoTracking()

                   // Actor = estudiante
               join st in _context.Actor
                   on r.StudentId equals st.Id

               // Actor = empresa
               join comp in _context.Actor
                   on r.CompanyId equals comp.Id

               // Debe existir ya la Práctica Pre Profesional
               join ppi in _context.PreProfessionalInternship
                   on r.Id equals ppi.RequestPPPId

               // Debe existir el vínculo con el contrato
               join link in _context.PreProfessionalInternshipByAdvisoringContract
                   on ppi.Id equals link.PreProfessionalInternshipId

               // Debe existir el contrato de asesoría
               join ac in _context.AdvisoringContract
                   on link.AdvisoringContractId equals ac.Id

               // Debe existir el actor que es asesor
               join ad in _context.Actor
                   on ac.AdvisorId equals ad.Id

               select new ListResolutionRequestPPPDto
               {
                   RequestPPPId = r.Id,
                   StudentCode  = st.Code!,
                   Student      = $"{st.FirstName} {st.SecondName}",
                   Advisor      = $"{ad.FirstName} {ad.SecondName}",
                   Company      = comp.FirstName!,
                   Topic        = r.Title!,
                   StartDate    = r.StartPreProfessionalPractice.HasValue
                                      ? r.StartPreProfessionalPractice.Value.ToString("dd-MM-yyyy")
                                      : string.Empty,
                   EndDate      = r.EndPreProfessionalPractice.HasValue
                                      ? r.EndPreProfessionalPractice.Value.ToString("dd-MM-yyyy")
                                      : string.Empty,
               };

                    return await query.ToListAsync();




        }
    }
}
