//using GestionAsesoria.Operator.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest
//{
//    public interface IGoogleCalendarService
//    {
//        Task<string> AuthorizeAsync(string userId);
//        Task<GoogleCalendarEvent> CreateEventAsync(GoogleCalendarEvent calendarEvent, string userId);
//        Task<List<GoogleCalendarEvent>> GetEventsAsync(DateTime startDate, DateTime endDate, string userId);
//        Task<bool> DeleteEventAsync(string eventId, string userId);
//        Task<GoogleCalendarEvent> UpdateEventAsync(GoogleCalendarEvent calendarEvent, string userId);

//        // Nuevos métodos para gestión de citas
//        Task<AppointmentSchedule> ScheduleAppointmentAsync(AppointmentSchedule appointment, string userId);
//        Task<List<AppointmentSchedule>> GetAdvisorAvailabilityAsync(string advisorId, DateTime startDate, DateTime endDate, string userId);
//        Task<bool> CancelAppointmentAsync(string appointmentId, string reason, string userId);
//        Task<AppointmentSchedule> RescheduleAppointmentAsync(string appointmentId, DateTime newStartTime, DateTime newEndTime, string userId);
//        Task<List<AppointmentSchedule>> GetUserAppointmentsAsync(string userId, string role, DateTime startDate, DateTime endDate);
//        Task<bool> ConfirmAppointmentAsync(string appointmentId, string userId);
//        Task<List<TimeSlot>> GetAvailableTimeSlotsAsync(string advisorId, DateTime date, string userId);
//    }

//    public class TimeSlot
//    {
//        public DateTime StartTime { get; set; }
//        public DateTime EndTime { get; set; }
//        public bool IsAvailable { get; set; }
//    }
//}
