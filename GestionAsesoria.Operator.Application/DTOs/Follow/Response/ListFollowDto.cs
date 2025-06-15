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
        public string Status { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int DurationDays {  get; set; }
        public int DaysRemaining { get; set; }
        public double ProgressPercentage { get; set; }


    }
}
