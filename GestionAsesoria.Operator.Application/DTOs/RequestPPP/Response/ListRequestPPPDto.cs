using System;

namespace GestionAsesoria.Operator.Application.DTOs.RequestPPP.Response
{
    public class ListRequestPPPDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Modality { get; set; }
        public string Status { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? StartPreProfessionalPractice { get; set; }
        public DateTime? EndPreProfessionalPractice { get; set; }

        public string StudentName { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string AcademicAreaName { get; set; } = null!;

        public string DocumentUrl { get; set; } = null!;
    }
}
