using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchArea;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchGroup;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchLine;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorTeacher;
using GestionAsesoria.Operator.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.Interfaces.Repositories
{
    public interface IActorRepositoryAsync : IGenericRepositoryAsync<Actor, int>
    {
        // Consultas básicas
        Task<List<Actor>> GetActorsByRoleAndStatusAsync(int roleId, bool isActive);
        Task<Actor> GetActorByIdWithDetailsAsync(int actorId);
        Task<Actor> GetActorWithDetailsAsync(int actorId);

        // Consultas relacionadas con grupos, áreas y lineas de investigación
        Task<List<Actor>> GetActiveResearchGroupsAsync();
        Task<Actor> GetResearchGroupByIdAsync(int? groupId);
        Task<List<GetActorResearchGroupDto>> GetResearchGroupsAsync();
        Task<List<GetActorResearchAreaDto>> GetResearchAreasAsync(int? groupId);
        Task<List<GetActorResearchLineDto>> GetResearchLinesAsync(int? groupId);
        Task<List<GetActorTeacherDto>> GetTeachersAsync(int? groupId);


        Task<Actor> GetResearchGroupWithDetailsAsync(int researchGroupId, int researchLineId, int researchAreaId, int actorId);

        //// Consultas relacionadas con membresías
        Task<List<Membership>> GetActiveMembershipsByAdvisorAsync(int advisorId);
        // Consultas de jerarquía
        Task<List<ActorResponseDto>> GetChildActorsByParentAndRoleAsync(int parentId, int roleId);
        //Task<IEnumerable<Actor>> GetActorWithDetailsByRolesAsync(int actorId);
    }
}
