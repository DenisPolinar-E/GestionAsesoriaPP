using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.DTOs.Actor.Response
{
    public class ActorTeacherCountDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int PracticeCount { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public string BranchName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
