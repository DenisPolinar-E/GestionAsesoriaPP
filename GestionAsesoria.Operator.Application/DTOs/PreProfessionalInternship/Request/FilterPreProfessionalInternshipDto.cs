using System;

namespace GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request
{
    public class FilterPreProfessionalInternshipDto
    {
        public string? StudentCode { get; set; }
        public string? StudentName { get; set; }
        public string? AdvisorName { get; set; } // Nombre del asesor asignado
        public string? CompanyName { get; set; }
        public DateTime? StartPreProfessionalPractice { get; set; }
        public DateTime? EndPreProfessionalPractice { get; set; }
        public string? Status { get; set; }
    }
}