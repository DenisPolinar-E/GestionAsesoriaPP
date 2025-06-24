using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request;
using System;

namespace GestionAsesoria.Operator.Application.DTOs.ScientificProduction.Request
{
    public class CreateScientificProductionRequestDto
    {
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string? Doi { get; set; }
        public string Code { get; set; }
        public int ProjectId { get; set; }
        public int ProductionTypeId { get; set; }
        public CreateDocumentCollectionRequestDto DeliverableDocumentCollection { get; set; }
    }
} 