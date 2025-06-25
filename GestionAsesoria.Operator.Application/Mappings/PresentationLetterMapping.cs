// GestionAsesoria.Operator.Application/Mappings/PresentationLetterMapping.cs
using AutoMapper;
using GestionAsesoria.Operator.Domain.Entities;
using GestionAsesoria.Operator.Application.DTOs.PresentationLetter.Request;
using GestionAsesoria.Operator.Application.DTOs.PresentationLetter.Response;

namespace GestionAsesoria.Operator.Application.Mappings
{
    public class PresentationLetterMapping : Profile
    {
        public PresentationLetterMapping()
        {
            CreateMap<CreatePresentationLetterDto, PresentationLetter>();
            CreateMap<PresentationLetter, PresentationLetterDto>();
        }
    }
}