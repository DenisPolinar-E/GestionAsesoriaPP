using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.DTOs.Funding.Request
{
    public class CreateFundingRequestDto
    {
        // BÁSICO
        public decimal Amount { get; set; }
        public string FundingType { get; set; }
        public bool IsCompetitiveFund { get; set; }
        public string FundingName { get; set; }
        public string Organization { get; set; }

        // LLAVE FORÁNEA
        public int ProjectId { get; set; }
    }
}
