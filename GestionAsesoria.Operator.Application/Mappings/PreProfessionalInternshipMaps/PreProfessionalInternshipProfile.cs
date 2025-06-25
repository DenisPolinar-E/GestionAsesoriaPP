using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Domain.Entities;

namespace GestionAsesoria.Operator.Application.Mappings
{
    public class PreProfessionalInternshipProfile : Profile
    {
        public PreProfessionalInternshipProfile()
        {
            CreateMap<PreProfessionalInternship, PreProfessionalInternshipDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.RequestPPP.Student.Code))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => $"{src.RequestPPP.Student.FirstName} {src.RequestPPP.Student.SecondName}"))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.RequestPPP.Company.FirstName))
                //.ForMember(dest => dest.Career, opt => opt.MapFrom(src => src.RequestPPP.ResearchArea.FirstName))
                .ForMember(dest => dest.StartPreProfessionalPractice, opt => opt.MapFrom(src => src.RequestPPP.StartPreProfessionalPractice))
                .ForMember(dest => dest.EndPreProfessionalPractice, opt => opt.MapFrom(src => src.RequestPPP.EndPreProfessionalPractice))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.RequestPPP.Status.Value));
            // .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.RequestPPP.Note)); // Mapeará cuando se implemente
        }
    }
}