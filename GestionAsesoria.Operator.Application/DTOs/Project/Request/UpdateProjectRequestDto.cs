using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request;
using System;

namespace GestionAsesoria.Operator.Application.DTOs.Project.Request
{
    public class UpdateProjectRequestDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ExecutionPlace { get; set; }
        public bool IsActived { get; set; }

        // Relaciones obligatorias
        public int ResearchGroupProjectId { get; set; }
        public int ResearchAreaProjectId { get; set; }
        public int ResearchLineProjectId { get; set; }
        public int MethodProjectId { get; set; }
        public int ClassificationProjectId { get; set; }

        // Relaciones opcionales
        public int? OdsObjectiveId { get; set; }

        // Documentos opcionales para actualización
        public int? ResolutionDocumentCollectionId { get; set; }
        public int? PlanDocumentCollectionId { get; set; }
        public int? ReportAdvisorDocumentCollectionId { get; set; }
        public int? ReportOfResearchGroupDocumentCollectionId { get; set; }
        public int? ReportOfDegreesAndTittlesCommitteeId { get; set; }
    }
} 