using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.ProjectActor.Request;
using GestionAsesoria.Operator.Domain.Entities.ProjectIDI;

namespace GestionAsesoria.Operator.Application.Mappings.ProjectActors
{
    public class ProjectActorProfile : Profile
    {
        public ProjectActorProfile()
        {
            CreateMap<CreateProjectActorRequestDto, ProjectActor>();
        }
    }
} 