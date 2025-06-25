// GestionAsesoria.Operator.Domain/Entities/PresentationLetter.cs
using GestionAsesoria.Operator.Domain.Auditable;
using Microsoft.EntityFrameworkCore;

namespace GestionAsesoria.Operator.Domain.Entities
{
    [Comment("Representa una carta de presentación para un alumno de prácticas")]
    public class PresentationLetter : AuditableEntity<int>
    {
        // Información del estudiante
        public string StudentCode { get; set; } = null!;
        public string StudentName { get; set; } = null!;
        public string StudentEmail { get; set; } = null!;
        public string Career { get; set; } = null!;

        // Información de la empresa
        public string CompanyName { get; set; } = null!;
        public string CompanyRuc { get; set; } = null!;
        public string CompanyAddress { get; set; } = null!;



        // Relaciones
        public int StudentId { get; set; }
        public int RequestPPPId { get; set; }

        // Navegación
        public virtual Actor Student { get; set; } = null!;
        public virtual RequestPPP RequestPPP { get; set; } = null!;
    }
}