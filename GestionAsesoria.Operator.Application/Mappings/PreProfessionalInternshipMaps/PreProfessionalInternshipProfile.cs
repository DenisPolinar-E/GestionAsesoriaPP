using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request;
using GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response;
using GestionAsesoria.Operator.Domain.Entities;
using System;
using System.Linq;

namespace GestionAsesoria.Operator.Application.Mappings
{
    public class PreProfessionalInternshipProfile : Profile
    {
        public PreProfessionalInternshipProfile()
        {
            // Mapeo para PreProfessionalInternshipDto (usado en GetAll)
            CreateMap<PreProfessionalInternship, PreProfessionalInternshipDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.RequestPPP != null && src.RequestPPP.Student != null ? src.RequestPPP.Student.Code : "Sin código"))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.RequestPPP != null && src.RequestPPP.Student != null 
                    ? $"{src.RequestPPP.Student.FirstName} {src.RequestPPP.Student.SecondName}" 
                    : "Sin estudiante"))
                .ForMember(dest => dest.AdvisorName, opt => opt.MapFrom(src => GetAdvisorName(src)))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.RequestPPP != null && src.RequestPPP.Company != null ? src.RequestPPP.Company.FirstName : "Sin empresa"))
                .ForMember(dest => dest.StartPreProfessionalPractice, opt => opt.MapFrom(src => src.RequestPPP != null ? src.RequestPPP.StartPreProfessionalPractice : (DateTime?)null))
                .ForMember(dest => dest.EndPreProfessionalPractice, opt => opt.MapFrom(src => src.RequestPPP != null ? src.RequestPPP.EndPreProfessionalPractice : (DateTime?)null))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => DetermineStatus(src.RequestPPP != null ? src.RequestPPP.StartPreProfessionalPractice : null, src.RequestPPP != null ? src.RequestPPP.EndPreProfessionalPractice : null)));
                // .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Evaluation?.Note)); // Descomentar cuando se implemente la tabla de notas

            // Mapeo para FilterPreProfessionalInternshipDto (usado en GetFiltered)
            CreateMap<PreProfessionalInternship, FilterPreProfessionalInternshipDto>()
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.RequestPPP != null && src.RequestPPP.Student != null ? src.RequestPPP.Student.Code : "Sin código"))
                .ForMember(dest => dest.AdvisorName, opt => opt.MapFrom(src => GetAdvisorName(src)))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.RequestPPP != null && src.RequestPPP.Company != null ? src.RequestPPP.Company.FirstName : "Sin empresa"))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => DetermineStatus(src.RequestPPP != null ? src.RequestPPP.StartPreProfessionalPractice : null, src.RequestPPP != null ? src.RequestPPP.EndPreProfessionalPractice : null)));
        }

        private string GetAdvisorName(PreProfessionalInternship src)
        {
            var contract = src.PreProfessionalInternshipContracts?.FirstOrDefault();
            var advisor = contract?.AdvisoringContract?.AdvisorActor;
            return advisor != null ? $"{advisor.FirstName} {advisor.SecondName}" : "Sin asesor";
        }

        private string DetermineStatus(DateTime? startDate, DateTime? endDate)
        {
            var currentDate = DateTime.Now;

            if (!startDate.HasValue || !endDate.HasValue)
                return "Sin estado";

            if (currentDate < startDate)
                return "Aprobada";
            else if (currentDate >= startDate && currentDate <= endDate)
                return "En Curso";
            else
                return "Finalizada";
            // Nota: Cuando se implemente la tabla de notas, agregar lógica para "Práctica Aprobada" si la nota es >= 11.
        }
    }
}