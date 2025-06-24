using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.DTOs.Actor.Response
{
    public class CompanyPracticeCountDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public int PracticeCount { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
