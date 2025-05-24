using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request;
using System;

namespace GestionAsesoria.Operator.Application.DTOs.ProjectEvaluation.Request
{
    public class CreateProjectEvaluationRequestDto
    {
        public DateTime EvaluationDate { get; set; }
        public string Comments { get; set; }
        public int ProjectId { get; set; }
        public int ActorId { get; set; }
        public int StatusEvaluationId { get; set; }
        public CreateDocumentCollectionRequestDto DocumentCollection { get; set; }
    }
} 