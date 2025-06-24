using System;

namespace GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response
{
    public class PreProfessionalInternshipDto
    {
        public string CodeStudentId { get; set; } = "";
        public string FullNameInterId { get; set; } = "";
        public string FullNameAdviserId { get; set; } = "";
        public string FullNameCompanyId { get; set; } = "";
        public string Career { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string State { get; set; } = "";
        public string? Note { get; set; }
    }
}
