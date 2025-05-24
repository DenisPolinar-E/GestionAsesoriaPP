using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request;
using System;

namespace GestionAsesoria.Operator.Application.DTOs.Advance.Request
{
    public class CreateAdvanceRequestDto
    {
        public DateTime AdvanceDate { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public CreateDocumentCollectionRequestDto DeliverableDocumentCollection { get; set; }
    }
} 