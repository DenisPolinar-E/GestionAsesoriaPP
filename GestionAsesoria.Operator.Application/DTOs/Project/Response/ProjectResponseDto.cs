using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Response;
using GestionAsesoria.Operator.Application.DTOs.MasterData.Response;
using System;
using System.Collections.Generic;

namespace GestionAsesoria.Operator.Application.DTOs.Project.Response
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ExecutionPlace { get; set; }
        public bool IsActived { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }

        // Propiedades de navegación obligatorias
        public int ResearchGroupProjectId { get; set; }
        public ActorResponseDto ResearchGroupProject { get; set; }
        public int ResearchAreaProjectId { get; set; }
        public ActorResponseDto ResearchAreaProject { get; set; }
        public int ResearchLineProjectId { get; set; }
        public ActorResponseDto ResearchLineProject { get; set; }
        public int MethodProjectId { get; set; }
        public MasterDataValueResponseDto MethodProject { get; set; }
        public int OdsObjectiveId { get; set; }
        public MasterDataValueResponseDto OdsObjective { get; set; }
        public int ClassificationProjectId { get; set; }
        public MasterDataValueResponseDto ClassificationProject { get; set; }
        public int StateProjectId { get; set; }
        public MasterDataValueResponseDto StateProject { get; set; }
        public int AuthorProjectId { get; set; }
        public ActorResponseDto AuthorProject { get; set; }

        // Propiedades de navegación opcionales
        public int? ResolutionDocumentCollectionId { get; set; }
        public DocumentCollectionResponseDto? ResolutionDocumentCollection { get; set; }
        public int? PlanDocumentCollectionId { get; set; }
        public DocumentCollectionResponseDto? PlanDocumentCollection { get; set; }
        public int? ReportAdvisorDocumentCollectionId { get; set; }
        public DocumentCollectionResponseDto? ReportAdvisorDocumentCollection { get; set; }
        public int? ReportOfResearchGroupDocumentCollectionId { get; set; }
        public DocumentCollectionResponseDto? ReportOfResearchGroupDocumentCollection { get; set; }
        public int? ReportOfDegreesAndTittlesCommitteeId { get; set; }
        public DocumentCollectionResponseDto? ReportOfDegreesAndTittlesCommittee { get; set; }

        // Colecciones
        public ICollection<AdvanceResponseDto>? Advances { get; set; }
        public ICollection<ProjectActorResponseDto>? ProjectActors { get; set; }
        public ICollection<ProjectEvaluationResponseDto>? ProjectEvaluations { get; set; }
        public ICollection<ScientificProductionResponseDto>? ScientificProductions { get; set; }
        public ICollection<FundingResponseDto>? Fundings { get; set; }
    }
} 