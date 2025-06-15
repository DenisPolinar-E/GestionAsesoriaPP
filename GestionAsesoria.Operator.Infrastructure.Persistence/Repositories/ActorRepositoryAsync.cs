using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchArea;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchGroup;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchLine;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorTeacher;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Domain.ConfigParameters.Container;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Infrastructure.Persistence.Contexts;
using GestionAsesoria.Operator.Infrastructure.Persistence.Repository;
using GestionAsesoria.Operator.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tsp.Sigescom.Config;

namespace GestionAsesoria.Operator.Infrastructure.Persistence.Repositories
{
    public class ActorRepositoryAsync : GenericRepositoryAsync<Actor, int>, IActorRepositoryAsync
    {
        private readonly SettingsContainer _settingsContainer;
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Actor> _actors;
        private readonly DbSet<Membership> _memberships;

        public ActorRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _settingsContainer = LocalSettingContainer.Get();
            _context = dbContext;
            _actors = _context.Set<Actor>();
            _memberships = _context.Set<Membership>();

        }
        public async Task<Actor> GetByIdentificationNumberAsync(string identificationNumber)
        {
            return await _actors.FirstOrDefaultAsync(a => a.IdentificationNumber == identificationNumber);
        }



        // Consultas LINQ puras
        public async Task<List<Actor>> GetActorsByRoleAndStatusAsync(int roleId, bool isActive)
        {
            return await _actors
                .Where(a => a.MainRoleId == roleId && a.IsActived == isActive)
                .ToListAsync();
        }

        public async Task<Actor> GetActorByIdWithDetailsAsync(int actorId)
        {
            return await _actors
                .Include(a => a.ActorType)
                .Include(a => a.MainRole)
                .Include(a => a.Parent)
                .FirstOrDefaultAsync(a => a.Id == actorId);
        }


        public async Task<List<Actor>> GetActiveResearchGroupsAsync()
        {
            return await _actors
                .Where(a => a.MainRoleId == 11 && a.IsActived) // 11 = Grupo de Investigación
                .ToListAsync();
        }

        public async Task<List<Membership>> GetActiveMembershipsByAdvisorAsync(int advisorId)
        {
            return await _memberships
                .Where(m => m.ActorId == advisorId && m.IsActived)
                .Include(m => m.MemberActor)
                .ToListAsync();
        }
        public async Task<Actor> GetByCodeAsync(string code)
        {
            return await _actors.FirstOrDefaultAsync(a => a.Code == code);
        }

        public async Task<Actor> GetResearchGroupByIdAsync(int? groupId)
        {
            if (!groupId.HasValue)
                return null;

            return await _actors
                .FirstOrDefaultAsync(a => a.Id == groupId.Value &&
                                        a.MainRoleId == 11 &&
                                        a.IsActived);
        }

        public async Task<List<ActorResponseDto>> GetChildActorsByParentAndRoleAsync(int parentId, int roleId)
        {
            return await _actors
                .Where(a => a.ParentId == parentId &&
                           a.MainRoleId == roleId &&
                           a.IsActived)
                .Select(a => new ActorResponseDto
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    MainRoleId = a.MainRoleId,
                    RoleName = a.MainRole.Name,
                    IsActived = a.IsActived
                })
                .ToListAsync();
        }

        //public async Task<List<Membership>> GetMembershipsByFiltersAsync(
        //    int? memberId = null,
        //    int? organizationId = null,
        //    bool? isActive = null)
        //{
        //    var query = _memberships.AsQueryable();

        //    if (memberId.HasValue)
        //        query = query.Where(m => m.ActorId == memberId);

        //    if (organizationId.HasValue)
        //        query = query.Where(m => m.OrganizationActorId == organizationId);

        //    if (isActive.HasValue)
        //        query = query.Where(m => m.IsActived == isActive);

        //    return await query
        //        .Include(m => m.MemberActor)
        //        .ToListAsync();
        //}

        ////public async Task<List<Membership>> GetTeacherMembershipsByGroupAsync(int groupId)
        ////{
        ////    return await _memberships
        ////        .Include(m => m.MemberActor)
        ////        .Where(m => m.OrganizationActorId == groupId &&
        ////                   m.IsActived &&
        ////                   m.MemberActor.MainRoleId == 15 &&
        ////                   m.MemberActor.IsActived)
        ////        .ToListAsync();
        ////}


        public async Task<Actor> GetResearchGroupWithDetailsAsync(int researchGroupId, int researchLineId, int researchAreaId, int actorId)
        {
            return await _actors
                .Where(a => a.Id == researchGroupId &&
                           a.MainRoleId == 11 &&
                           a.IsActived)
                .FirstOrDefaultAsync();
        }

        //public async Task<Actor> GetResearchGroupByAdvisorIdAsync(int advisorId)
        //{
        //    var membership = await _memberships
        //        .Where(m => m.ActorId == advisorId && m.IsActived)
        //        .FirstOrDefaultAsync();

        //    if (membership == null)
        //        return null;

        //    return await _actors
        //        .Where(a => a.Id == membership.OrganizationActorId &&
        //                   a.MainRoleId == 11 &&
        //                   a.IsActived)
        //        .FirstOrDefaultAsync();
        //}

        //public async Task<bool> IsStudentMemberOfResearchGroupAsync(int studentId, int researchGroupId)
        //{
        //    return await _memberships
        //        .AnyAsync(m => m.ActorId == studentId &&
        //                      m.OrganizationActorId == researchGroupId &&
        //                      m.IsActived);
        //}

        //public async Task AddStudentToResearchGroupAsync(int studentId, int researchGroupId, int membershipTypeId, int actorTypeId)
        //{
        //    var membership = new Membership
        //    {
        //        ActorId = studentId,
        //        OrganizationActorId = researchGroupId,
        //        MembershipTypeId = membershipTypeId,
        //        StartDate = DateTime.Now,
        //        IsActived = true
        //    };

        //    await _memberships.AddAsync(membership);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task<int?> GetStudentCurrentResearchGroupAsync(int studentId)
        //{
        //    var membership = await _memberships
        //        .Where(m => m.ActorId == studentId && m.IsActived)
        //        .Select(m => m.OrganizationActorId)
        //        .FirstOrDefaultAsync();

        //    return membership;
        //}

        //public async Task UpdateStudentResearchGroupAsync(int studentId, int newResearchGroupId)
        //{
        //    var currentMemberships = await _memberships
        //        .Where(m => m.ActorId == studentId && m.IsActived)
        //        .ToListAsync();

        //    foreach (var membership in currentMemberships)
        //    {
        //        membership.IsActived = false;
        //        membership.EndDate = DateTime.Now;
        //    }

        //    var newMembership = new Membership
        //    {
        //        ActorId = studentId,
        //        OrganizationActorId = newResearchGroupId,
        //        StartDate = DateTime.Now,
        //        IsActived = true
        //    };

        //    await _memberships.AddAsync(newMembership);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task AddMembershipAsync(Membership membership)
        //{
        //    await _memberships.AddAsync(membership);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task UpdateMembershipAsync(Membership membership)
        //{
        //    _context.Entry(membership).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //}

        public async Task<Actor> GetActorWithDetailsAsync(int actorId)
        {
            return await _actors
                .Include(a => a.ActorType)
                .Include(a => a.MainRole)
                .Where(a => a.Id == actorId && a.IsActived)
                .FirstOrDefaultAsync();
        }

        public async Task<List<GetActorResearchGroupDto>> GetResearchGroupsAsync()
        {
            var researhGroup = _settingsContainer.LocalResearchGroupSettings.ResearchGroupId;
            return await _actors
                 .Where(a => a.MainRoleId == _settingsContainer.LocalResearchGroupSettings.ResearchGroupId && a.IsActived)
                 .Select(a => new GetActorResearchGroupDto
                 {
                     Id = a.Id,
                     SecondName = a.SecondName
                 })
                 .ToListAsync();
        }

        public async Task<List<GetActorResearchAreaDto>> GetResearchAreasAsync(int? groupId)
        {
            var researhArea = _settingsContainer.LocalResearchAreaSettings.ResearchAreaId;
            var query = _actors.Where(a => a.MainRoleId == researhArea && a.IsActived);

            if (groupId.HasValue)
            {
                query = query.Where(a => a.ParentId == groupId.Value);
            }

            return await query
                .Select(a => new GetActorResearchAreaDto
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                })
                .ToListAsync();
        }
        public async Task<List<GetAllActorResearchAreaDto>> GetAllResearchAreasAsync()
        {
            var roleId = _settingsContainer.LocalResearchAreaSettings.ResearchAreaId;
            return await _actors
                .Where(a => a.IsActived && a.MainRoleId == roleId) // roleId 14 es Área de Investigación
                .Select(a => new GetAllActorResearchAreaDto
                {
                    Id = a.Id,
                    FirstName = a.FirstName
                })
                .ToListAsync();
        }


        public async Task<List<GetActorResearchLineDto>> GetResearchLinesAsync(int? groupId)
        {
            var researhLine = _settingsContainer.LocalResearchLineSettings.ResearchLineId;
            var query = _actors.Where(a => a.MainRoleId == researhLine && a.IsActived);

            if (groupId.HasValue)
            {
                query = query.Where(a => a.ParentId == groupId.Value);
            }

            return await query
                .Select(a => new GetActorResearchLineDto
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                })
                .ToListAsync();
        }

        public async Task<List<GetActorTeacherDto>> GetTeachersAsync(int? groupId)
        {
            var actorTeacher = _settingsContainer.LocalDocenteSettings.RoleDocenteId;
            int maxAdvisees = 6;

            var teachersQuery = _actors.Where(a => a.MainRoleId == actorTeacher && a.IsActived);

            if (groupId.HasValue)
            {
                teachersQuery = teachersQuery.Where(a => a.ParentId == groupId.Value);
            }

            var teachers = await teachersQuery.ToListAsync();

            var contracts = await _context.AdvisoringContract
                .Where(c => c.IsActived)
                .ToListAsync();

            var result = teachers.Select(teacher =>
            {
                var teacherContracts = contracts.Where(c => c.AdvisorId == teacher.Id);

                var researchGroupName = teacherContracts
                    .Where(c => c.ResearchGroupId != null)
                    .Select(c =>
                        _context.Actor
                            .Where(rg => rg.Id == c.ResearchGroupId && rg.MainRoleId == 11)
                            .Select(rg => rg.FirstName)
                            .FirstOrDefault()
                    )
                    .FirstOrDefault();

                var currentAdvisees = teacherContracts.Count();

                return new GetActorTeacherDto
                {
                    Id = teacher.Id,
                    Code = teacher.Code, // Cambia por el campo real de código si fuera diferente
                    FullName = teacher.FirstName, // Cambia si tienes un campo de nombre completo
                    ResearchGroup = researchGroupName ?? "Sin grupo",
                    InstitutionalEmail = teacher.Email, // Cambia por el campo real de email
                    CurrentAdvisees = currentAdvisees,
                    Availability = currentAdvisees < maxAdvisees ? "Disponible" : "Lleno"
                };
            }).ToList();

            return result;
        }

    }
}
