using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Application.DTOs.DocumentCollection.Response;
using GestionAsesoria.Operator.Application.DTOs.MasterDataValues;
using System;

namespace GestionAsesoria.Operator.Application.DTOs.ProjectEvaluation.Response
{
    public class ProjectEvaluationResponseDto
    {
        public int Id { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string Comments { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }

        // Propiedades de navegación
        public int ProjectId { get; set; }
        public int ActorId { get; set; }
        public ActorResponseDto Actor { get; set; }
        public int StatusEvaluationId { get; set; }
        public MasterDataValueResponseDto StatusEvaluation { get; set; }
        public int DocumentCollectionId { get; set; }
        public DocumentCollectionResponseDto DocumentCollection { get; set; }
    }
} 