using GestionAsesoria.Operator.Domain.Auditable;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GestionAsesoria.Operator.Domain.Entities
{
    [Comment("Permite crear una cita, incluyendo detalles sobre la fecha, ubicación y estado actual.")]
    public class Appointment : AuditableEntity<int>
    {
        public Appointment()
        {
            AdvisingSessions = new HashSet<AdvisingSession>();
        }

        [Comment("Fecha y hora de la cita")]
        public DateTime DateTime { get; set; }

        [Comment("Ubicación de la cita (física o virtual)")]
        public string Location { get; set; }

        [Comment("Título de la cita")]
        public string Title { get; set; }

        [Comment("Descripción de la cita")]
        public string Description { get; set; }

        [Comment("Enlace de reunión virtual (Google Meet, Zoom, etc.)")]
        public string? MeetingLink { get; set; }

        [Comment("Zona horaria de la cita")]
        public string TimeZone { get; set; } = "America/Bogota";

        [Comment("ID del evento en Google Calendar para sincronización")]
        public string? GoogleEventId { get; set; }

        [Comment("Información adicional sobre la cita")]
        public string? Notes { get; set; }

        [Comment("Tipo de cita desde MasterDataValue (Tesis, PPP, Tutoría, etc.)")]
        public int AppointmentTypeId { get; set; }

        [Comment("Estado actual de la cita")]
        public int CurrentAppointmentStatusId { get; set; }

        [Comment("Actor que agenda la cita (Estudiante)")]
        public int StudentActorId { get; set; }

        [Comment("Actor que recibe la cita (Docente/Asesor)")]
        public int AdvisorActorId { get; set; }

        // Propiedades de navegación
        [Comment("Tipo de cita")]
        public virtual MasterDataValue AppointmentType { get; set; }

        [Comment("Estado actual de la cita")]
        public virtual CurrentAppointmentStatus CurrentAppointmentStatus { get; set; }

        [Comment("Estudiante que agenda")]
        public virtual Actor StudentActor { get; set; }

        [Comment("Asesor que recibe la cita")]
        public virtual Actor AdvisorActor { get; set; }

        [Comment("Sesiones de asesoría asociadas")]
        public virtual ICollection<AdvisingSession> AdvisingSessions { get; set; }
    }
}
