using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GestionAsesoria.Operator.Application.DTOs.ReviewCommittee.Request
{
    public class AssignReviewCommitteeDto
    {
        [Required(ErrorMessage = "El ID de la práctica preprofesional es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la práctica debe ser un valor positivo.")]
        public int PreProfessionalInternshipId { get; set; }

        [Required(ErrorMessage = "La lista de docentes revisores es requerida.")]
        [MinLength(2, ErrorMessage = "Debe asignar exactamente dos docentes.")]
        [MaxLength(2, ErrorMessage = "Debe asignar exactamente dos docentes.")]
        public List<int> ReviewerTeacherIds { get; set; } = new List<int>();
        public (bool IsValid, string ErrorMessage) Validate()
        {
            if (PreProfessionalInternshipId <= 0)
                return (false, "El ID de la práctica preprofesional debe ser un valor positivo.");

            if (ReviewerTeacherIds == null)
                return (false, "La lista de docentes revisores no puede ser nula.");

            if (ReviewerTeacherIds.Count != 2)
                return (false, "Debe asignar exactamente dos docentes.");

            if (ReviewerTeacherIds.Any(id => id <= 0))
                return (false, "Los IDs de los docentes deben ser valores positivos.");

            if (ReviewerTeacherIds.Distinct().Count() != 2)
                return (false, "No se puede asignar el mismo docente dos veces.");

            return (true, string.Empty);
        }
    }
}