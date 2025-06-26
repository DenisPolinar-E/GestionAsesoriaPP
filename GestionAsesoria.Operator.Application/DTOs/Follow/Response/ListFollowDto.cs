using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.DTOs.Follow.Response
{
    public class ListFollowDto
    {
        public string StudentName { get; set; } = null!;
        public string StudentEmail { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime? StartPreProfessionalPractice { get; set; }
        public DateTime? EndPreProfessionalPractice { get; set; }
        public int DurationDays { get; set; }
        public int DaysRemaining { get; set; }
        public int DaysElapsed { get; set; }
        public double ProgressPercentage { get; set; }

        public string AdvisorName { get; set; } = null!;
        public string AdvisorEmail { get; set; } = null!;

        // CommissionEmail mejor manejarlo por configuración, no aquí
    }

}
