using System;

namespace GestionAsesoria.Operator.Domain.Entities
{
    public class PreProfessionalInternship
    {
        public int Id { get; set; }
        public int InterId { get; set; }
        public string CodeStudentId { get; set; }
        public string FullNameInterId { get; set; }
        public int AdviserId { get; set; }
        public string FullNameAdviserId { get; set; }
        public int CompanyId { get; set; }
        public string FullNameCompanyId { get; set; }
        public string Career { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string State { get; set; }
        public decimal? Note { get; set; }
    }
}