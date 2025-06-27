using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.Follow.Response;
using GestionAsesoria.Operator.Domain.Entities;

public class FollowPPPMapping : Profile
{
    public FollowPPPMapping()
    {
        CreateMap<PreProfessionalInternship, ListFollowDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.RequestPPP.Student.FirstName} {src.RequestPPP.Student.SecondName}"))
            .ForMember(dest => dest.StartPreProfessionalPractice, opt => opt.MapFrom(src => src.RequestPPP.StartPreProfessionalPractice))
            .ForMember(dest => dest.EndPreProfessionalPractice, opt => opt.MapFrom(src => src.RequestPPP.EndPreProfessionalPractice));
    }
}