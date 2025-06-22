using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.Follow.Response;
using GestionAsesoria.Operator.Domain.Entities;

public class FollowPPPMapping : Profile
{
    public FollowPPPMapping()
    {
        CreateMap<PreProfessionalInternship, ListFollowDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.RequestPPP.Student.FirstName} {src.RequestPPP.Student.SecondName}"))
            .ForMember(dest => dest.StartRequest, opt => opt.MapFrom(src => src.RequestPPP.StartRequest))
            .ForMember(dest => dest.EndRequest, opt => opt.MapFrom(src => src.RequestPPP.EndRequest))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.RequestPPP.Status.Value));
    }
}