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
                    StartRequest = r.StartRequest,
                    EndRequest = r.EndRequest,

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


    }
}
