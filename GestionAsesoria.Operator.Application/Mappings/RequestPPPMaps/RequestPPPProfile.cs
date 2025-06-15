using AutoMapper;
using GestionAsesoria.Operator.Domain.Entities;

namespace GestionAsesoria.Operator.Application.Mappings.RequestPPPRequestPPPMaps
{
    public class RequestPPPProfile : Profile
    {
        public RequestPPPProfile()
        {
            CreateMap<CreateRequestPPPRequestDto, RequestPPP>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Plan, opt => opt.MapFrom(src => src.Plan))
                .ForMember(dest => dest.Functions, opt => opt.MapFrom(src => src.Functions))
                .ForMember(dest => dest.Modality, opt => opt.MapFrom(src => src.Modality))
                .ForMember(dest => dest.AssignedArea, opt => opt.MapFrom(src => src.AssignedArea))
                .ForMember(dest => dest.Observations, opt => opt.MapFrom(src => src.Observations))
                .ForMember(dest => dest.ResearchAreaId, opt => opt.MapFrom(src => src.ResearchAreaId))

                // Estas claves foráneas se asignan directamente en el comando
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.RepresentativeId, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentCollectionId, opt => opt.Ignore())
                .ForMember(dest => dest.StatusId, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyRepresentativeId, opt => opt.Ignore())
                // Ignorar navegación
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Company, opt => opt.Ignore())
                .ForMember(dest => dest.Representative, opt => opt.Ignore())
                .ForMember(dest => dest.ResearchArea, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentCollection, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyRepresentative, opt => opt.Ignore());
        }
    }
}
