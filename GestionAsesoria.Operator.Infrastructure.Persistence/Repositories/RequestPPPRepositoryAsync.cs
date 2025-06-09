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

        //public async Task<string> GetEstadoByIdAsync(int id)
        //{
        //    var estado = await _requestPPP.Where(x => x.Id == id).Select(x => x.Estado).FirstOrDefaultAsync();
        //    return estado;
        //}
        public async Task<List<ListRequestPPPDto>> GetAllForListAsync()
        {
         
            return await _context.RequestPPP
                .Include(r => r.Student) // navigation
                .Include(r => r.Company)
                .Include(r => r.Representative)
                .Include(r => r.ResearchArea)
                .Include(r => r.Status)
                .Select(r => new ListRequestPPPDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Functions = r.Functions,
                    Modality = r.Modality,
                    AssignedArea = r.AssignedArea,
                    Plan = r.Plan,
                    Observations = r.Observations,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate

                })
                .ToListAsync();
        }

    }
}
