using GestionAsesoria.Operator.Domain.Auditable;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestionAsesoria.Operator.Domain.Entities
{
    [Comment("Representa una solicitud de prácticas pre profesionales realizada por un estudiante.")]
    public class RequestPPP : AuditableEntity<int>
    {

        // ===================== CAMPOS PRINCIPALES =====================
        [Comment("Título del plan de prácticas pre profesionales.")]
        public string Title { get; set; } = null!;

        [Comment("Plan detallado de las prácticas.")]
        public string? Plan { get; set; }

        [Comment("Funciones asignadas al estudiante.")]
        public string? Functions { get; set; }

        [Comment("Modalidad de ejecución de las prácticas.")]
        public string? Modality { get; set; }

        [Comment("Área a la que fue asignado el estudiante.")]
        public string? AssignedArea { get; set; }

        [Comment("Observaciones generales sobre la solicitud.")]
        public string? Observations { get; set; }

        [Comment("Fecha prevista de inicio de prácticas.")]
        public DateTime? StartDate { get; set; }

        [Comment("Fecha prevista de fin de prácticas.")]
        public DateTime? EndDate { get; set; }

        // ===================== CLAVES FORÁNEAS =====================
        [Comment("ID del estudiante solicitante.")]
        public int StudentId { get; set; }

        [Comment("ID de la empresa donde se realizarán las prácticas.")]
        public int CompanyId { get; set; }

        [Comment("ID del representante legal de la empresa.")]
        public int RepresentativeId { get; set; }

        [Comment("ID del área académica asignada.")]
        public int ResearchAreaId { get; set; }

        [Comment("ID de la colección de documentos asociados.")]
        public int DocumentCollectionId { get; set; }

        [Comment("ID del estado actual de la solicitud.")]
        public int StatusId { get; set; }

        // ===================== RELACIONES DE NAVEGACIÓN =====================
        [Comment("Entidad Actor que representa al estudiante.")]
        public virtual Actor Student { get; set; } = null!;

        [Comment("Entidad Actor que representa la empresa receptora.")]
        public virtual Actor Company { get; set; } = null!;

        [Comment("Entidad Actor que representa al representante legal.")]
        public virtual Actor Representative { get; set; } = null!;

        [Comment("Entidad Actor que representa al área académica.")]
        public virtual Actor ResearchArea { get; set; } = null!;

        [Comment("Entidad que representa la colección de documentos asociados.")]
        public virtual DocumentCollection DocumentCollection { get; set; } = null!;

        [Comment("Entidad MasterDataValue que representa el estado de la solicitud.")]
        public virtual MasterDataValue Status { get; set; } = null!;
    }
}
