using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Domain.Entities;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories
{
    public interface IComitteReviewRepositoryAsync
    {
        Task<List<Actor>> GetAvailableTeachersAsync(int solicitudId);
        Task AssignCommitteeAsync(int solicitudId, int docente1Id, int docente2Id);
    }
}
