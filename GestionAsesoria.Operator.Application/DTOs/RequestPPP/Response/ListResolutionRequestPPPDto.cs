using Google.Apis.Logging;
using MimeKit.Tnef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.DTOs.RequestPPP.Response
{
    public class ListResolutionRequestPPPDto
    {
        public int RequestPPPId { get; set; }
        public string StudentCode { get; set; } = null!;
        public string Student { get; set; } = null!;
        public string Advisor { get; set; } = null!;
        public string Company { get; set; } = null!;
        public string Topic { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
    }
}
