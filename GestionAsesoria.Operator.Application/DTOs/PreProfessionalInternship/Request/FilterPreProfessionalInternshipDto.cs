using System;

namespace GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request
{
    public class FilterPreProfessionalInternshipDto
    {
        public string? StudentCode { get; set; }
        public string? AdvisorName { get; set; } // Nombre del asesor asignado
        public string? CompanyName { get; set; }
        public string? Status { get; set; }
    }
}