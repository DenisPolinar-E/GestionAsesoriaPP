using GestionAsesoria.Operator.Application.DTOs.Actor.Response;
using GestionAsesoria.Operator.Application.DTOs.MasterDataValues;
using System;

namespace GestionAsesoria.Operator.Application.DTOs.ProjectActor.Response
{
    public class ProjectActorResponseDto
    {
        public int Id { get; set; }
        public string Justification { get; set; }
        public int ProjectId { get; set; }
        public int ActorId { get; set; }
        public int AuthorTypeId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }

        // Propiedades de navegación
        public ActorResponseDto Actor { get; set; }
        public MasterDataValueResponseDto AuthorType { get; set; }
    }
} 