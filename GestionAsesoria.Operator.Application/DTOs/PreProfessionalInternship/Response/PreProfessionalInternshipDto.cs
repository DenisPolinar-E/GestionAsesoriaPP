using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Response
{
    public class PreProfessionalInternshipDto
    {
        public int Id { get; set; }
        public string StudentCode { get; set; } = "";
        public string StudentName { get; set; } = "";
        public string CompanyName { get; set; } = "";

        // cuando se cree la variable para asesor se colocara

        // public string Career { get; set; } = ""; // Asumiendo que ResearchArea.FirstName es la carrera
        public DateTime? StartPreProfessionalPractice { get; set; } //cmabiar inicio practica
        public DateTime? EndPreProfessionalPractice { get; set; }
        public string Status { get; set; } = "";

        // public decimal? Note { get; set; } // Nota vigesimal (0-20) asignada tras revisión
    }
}