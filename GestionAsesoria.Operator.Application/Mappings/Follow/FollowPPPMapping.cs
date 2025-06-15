using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.Follow.Response;
using GestionAsesoria.Operator.Domain.Entities;

public class FollowPPPMapping : Profile
{
    public FollowPPPMapping()
    {
        CreateMap<PreProfessionalInternship, ListFollowDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.StudentName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));
    }
}
