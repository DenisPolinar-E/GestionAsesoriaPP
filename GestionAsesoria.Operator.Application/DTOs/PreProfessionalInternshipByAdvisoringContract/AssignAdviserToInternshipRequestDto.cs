using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternshipByAdvisoringContract.Request
{
    public class AssignAdviserToInternshipRequestDto
    {
        public int PreProfessionalInternshipId { get; set; }
        public int AdvisoringContractId { get; set; }
    }
}
