using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.DTOs.Actor.Response
{
    public class TeacherAvailabilityCountDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ResearchGroup { get; set; }
        public int CountTeacher { get; set; }       // Total de asesorías asignadas
        public string Availability { get; set; }    // "Disponible" o "Lleno"
        public DateTime StartDate { get; set; }
    }
}
