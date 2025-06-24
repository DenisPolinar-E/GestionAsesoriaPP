using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.Funding.Request;
using GestionAsesoria.Operator.Application.DTOs.Funding.Response;
using GestionAsesoria.Operator.Domain.Entities.ProjectIDI;

namespace GestionAsesoria.Operator.Application.Mappings.Fundings
{
    public class FundingProfile : Profile
    {
        public FundingProfile()
        {
            CreateMap<CreateFundingRequestDto, Funding>();
            CreateMap<Funding, FundingResponseDto>();
        }
    }
} 