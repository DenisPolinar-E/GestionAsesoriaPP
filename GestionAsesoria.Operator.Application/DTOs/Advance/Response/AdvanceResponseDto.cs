using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Response;
using System;
using System.Collections.Generic;

namespace GestionAsesoria.Operator.Application.DTOs.Advance.Response
{
    public class AdvanceResponseDto
    {
        public int Id { get; set; }
        public DateTime AdvanceDate { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }

        // Propiedades de navegación
        public int ProjectId { get; set; }
        public int DeliverableDocumentCollectionId { get; set; }
        public DocumentCollectionResponseDto DeliverableDocumentCollection { get; set; }
        public int? ReportOfResearchGroupDocumentCollectionId { get; set; }
        public DocumentCollectionResponseDto? ReportOfResearchGroupDocumentCollection { get; set; }

        // Colecciones
        //public ICollection<AdvanceEvaluationResponseDto>? AdvanceEvaluations { get; set; }
    }
} 