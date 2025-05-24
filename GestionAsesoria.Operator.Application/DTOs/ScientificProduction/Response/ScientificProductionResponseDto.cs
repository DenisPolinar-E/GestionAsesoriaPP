using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Response;
using GestionAsesoria.Operator.Application.DTOs.MasterDataValues;
using System;
using System.Collections.Generic;

namespace GestionAsesoria.Operator.Application.DTOs.ScientificProduction.Response
{
    public class ScientificProductionResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string? Doi { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }

        // Propiedades de navegación
        public int ProjectId { get; set; }
        public int ProductionTypeId { get; set; }
        public MasterDataValueResponseDto ProductionType { get; set; }
        public int DeliverableDocumentCollectionId { get; set; }
        public DocumentCollectionResponseDto DeliverableDocumentCollection { get; set; }

        // Colecciones
        //public ICollection<ScientificProductionEvaluationResponseDto>? ScientificProductionEvaluations { get; set; }
    }
} 