using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Request;
using System;
using System.Collections.Generic;

namespace GestionAsesoria.Operator.Application.DTOs.Project.Request
{
    public class CreateProjectRequestDto
    {
        // Datos básicos del proyecto
        public int AuthorProjectId { get; set; }
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

        //public int PlanDocumentCollectionId { get; set; }
    }
}
