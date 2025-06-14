using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.DTOs.RequestPPP.Request
{
    public class AssignAdviserToRequestPPPRequestDto
    {
        public int RequestPPPId { get; set; }
        public int AdviserId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
    }
}
