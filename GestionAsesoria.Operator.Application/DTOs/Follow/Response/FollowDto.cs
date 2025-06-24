using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.DTOs.Follow.Response
{
    public class FollowDto
    {
        public int Id { get; set; } // Identificador único de la práctica
        public string FullNameInterId { get; set; } // Nombre o ID del estudiante
        public string FullNameAdviserId { get; set; } // ID del asesor (nullable)
        public string FullNameCompanyId { get; set; } // Nombre o ID de la empresa donde se realiza la práctica
        public DateTime StartDate { get; set; } // Fecha de inicio
        public DateTime EndDate { get; set; } // Fecha de fin
        public string State { get; set; } // Estado de la práctica (ej. "EnCurso", "Finalizado")
        public int DurationDays { get; set; } // Duración total en días
        public int DaysElapsed { get; set; } // Días transcurridos desde StartDate hasta hoy
        public int DaysRemaining { get; set; } // Días restantes hasta EndDate
        public double ProgressPorcent { get; set; } // Porcentaje de progreso
    }
}
