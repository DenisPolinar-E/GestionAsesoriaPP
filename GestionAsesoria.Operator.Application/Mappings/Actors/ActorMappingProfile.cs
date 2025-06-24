using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchArea;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchGroup;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorResearchLine;
using GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorTeacher;
using GestionAsesoria.Operator.Domain.Entities;

namespace GestionAsesoria.Operator.Application.Mappings.Actors
{
    public class ActorMappingProfile : Profile
    {
        public ActorMappingProfile()
        {
            CreateMap<Actor, GetActorResearchGroupDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SecondName, opt => opt.MapFrom(src => src.SecondName));

            CreateMap<Actor, GetActorResearchAreaDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName));

            CreateMap<Actor, GetActorResearchLineDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName));

            CreateMap<Actor, GetActorTeacherDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.SecondName, opt => opt.MapFrom(src => src.SecondName));

            CreateMap<Actor, ActorResponseDto>()
            .ForMember(dest => dest.MainRoleId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.MainRole.Name)
            );
        }
    }
}
