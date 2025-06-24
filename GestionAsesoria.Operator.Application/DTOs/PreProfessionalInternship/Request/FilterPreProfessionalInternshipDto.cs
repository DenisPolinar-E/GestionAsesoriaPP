namespace GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request
{
    public class FilterPreProfessionalInternshipDto
    {
        public string CodeStudentId { get; set; }
        public string FullNameAdviserId { get; set; }
        public string FullNameCompanyId { get; set; }
        public string State { get; set; }
    }
}