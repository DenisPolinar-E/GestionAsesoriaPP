using System;

namespace GestionAsesoria.Operator.Application.DTOs.Funding.Response
{
    public class FundingResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string FundingType { get; set; }
        public bool IsCompetitiveFund { get; set; }
        public string FundingName { get; set; }
        public string Organization { get; set; }
        public int ProjectId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
    }
} 