namespace GestionAsesoria.Operator.Application.DTOs.Actor.Response.ActorTeacher
{
    public class GetActorTeacherDto
    {
        public int Id { get; set; }
        public string Code { get; set; } // Código del docente
        public string FirstName { get; set; } // Nombre
        public string SecondName { get; set; }
        public string FullName { get; set; } // Apellido y Nombre
        public string ResearchGroup { get; set; }
        public string InstitutionalEmail { get; set; }
        public int CurrentAdvisees { get; set; }
        public string Availability { get; set; } // Disponible o Lleno
    }
}
