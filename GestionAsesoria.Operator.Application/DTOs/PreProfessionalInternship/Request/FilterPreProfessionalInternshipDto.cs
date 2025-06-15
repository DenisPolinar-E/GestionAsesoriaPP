using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.DTOs.PreProfessionalInternship.Request
{
    public class FilterPreProfessionalInternshipDto
    {
        public string? StudentCode { get; set; }
        // CAMBIAR CON VARIABLE CUANDO CREER POR NOMBRE DE ASESOR public string? RepresentativeName { get; set; }
        public string? CompanyName { get; set; }
        public string? Status { get; set; }
    }
}
