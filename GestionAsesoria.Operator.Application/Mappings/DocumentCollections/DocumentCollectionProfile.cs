using AutoMapper;
using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request;
using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Response;
using GestionAsesoria.Operator.Domain.Entities;

namespace GestionAsesoria.Operator.Application.Mappings.DocumentCollections
{
    public class DocumentCollectionProfile : Profile
    {
        public DocumentCollectionProfile()
        {
            CreateMap<CreateDocumentCollectionRequestDto, DocumentCollection>();
            CreateMap<DocumentCollection, DocumentCollectionResponseDto>();
            
            // Mapeo del DTO simplificado
            CreateMap<CreatePlanDocumentRequestDto, CreateDocumentCollectionRequestDto>();
            CreateMap<CreatePlanDocumentRequestDto, DocumentCollection>();
        }
    }
} 
