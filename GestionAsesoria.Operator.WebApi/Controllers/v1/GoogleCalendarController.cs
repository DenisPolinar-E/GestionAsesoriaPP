//using GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest;
//using GestionAsesoria.Operator.Domain.Entities;
//using GestionAsesoria.Operator.Shared.Constants.Permission;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading.Tasks;

//namespace GestionAsesoria.Operator.WebApi.Controllers.v1
//{
//    public class GoogleCalendarController : BaseApiController<GoogleCalendarController>
//    {
//        private readonly IGoogleCalendarService _googleCalendarService;
//        private readonly ILogger<GoogleCalendarController> _logger;

//        public GoogleCalendarController(
//            IGoogleCalendarService googleCalendarService,
//            ILogger<GoogleCalendarController> logger)
//        {
//            _googleCalendarService = googleCalendarService;
//            _logger = logger;
//        }

//        /// <summary>
//        /// Autoriza el acceso a Google Calendar
//        /// </summary>
//        [HttpGet("authorize")]
//        [AllowAnonymous]
//        public async Task<IActionResult> Authorize([FromQuery] string userId)
//        {
//            try
//            {
//                _logger.LogInformation("Iniciando autorización para usuario {UserId}", userId);
//                var token = await _googleCalendarService.AuthorizeAsync(userId);
//                return Ok(new { Token = token });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error durante la autorización de Google Calendar");
//                return BadRequest(new { Message = "Error durante la autorización", Error = ex.Message });
//            }
//        }

//        #region Calendar Events
//        /// <summary>
//        /// Crea un evento en Google Calendar
//        /// </summary>
//        [Authorize(Policy = Permissions.GoogleCalendars.Create)]
//        [HttpPost("create")]
//        public async Task<IActionResult> CreateEvent(
//            [FromBody] GoogleCalendarEvent calendarEvent,
//            [FromQuery] string userId)
//        {
//            try
//            {
//                var createdEvent = await _googleCalendarService.CreateEventAsync(calendarEvent, userId);
//                return Ok(createdEvent);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al crear evento en Google Calendar");
//                return BadRequest(new { Message = "Error al crear evento", Error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Obtiene eventos de Google Calendar
//        /// </summary>
//        [Authorize(Policy = Permissions.GoogleCalendars.View)]
//        [HttpGet("events")]
//        public async Task<IActionResult> GetEvents(
//            [FromQuery] DateTime startDate,
//            [FromQuery] DateTime endDate,
//            [FromQuery] string userId)
//        {
//            try
//            {
//                var events = await _googleCalendarService.GetEventsAsync(startDate, endDate, userId);
//                return Ok(events);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener eventos");
//                return BadRequest(new { Message = "Error al obtener eventos", Error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Elimina un evento de Google Calendar
//        /// </summary>
//        [Authorize(Policy = Permissions.GoogleCalendars.Delete)]
//        [HttpDelete("delete/{eventId}")]
//        public async Task<IActionResult> DeleteEvent(
//            string eventId,
//            [FromQuery] string userId)
//        {
//            try
//            {
//                var result = await _googleCalendarService.DeleteEventAsync(eventId, userId);
//                return Ok(new { Success = result });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al eliminar evento de Google Calendar");
//                return BadRequest(new { Message = "Error al eliminar evento", Error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Actualiza un evento en Google Calendar
//        /// </summary>
//        [Authorize(Policy = Permissions.GoogleCalendars.Edit)]
//        [HttpPut("update")]
//        public async Task<IActionResult> UpdateEvent(
//            [FromBody] GoogleCalendarEvent calendarEvent,
//            [FromQuery] string userId)
//        {
//            try
//            {
//                var updatedEvent = await _googleCalendarService.UpdateEventAsync(calendarEvent, userId);
//                return Ok(updatedEvent);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al actualizar evento en Google Calendar");
//                return BadRequest(new { Message = "Error al actualizar evento", Error = ex.Message });
//            }
//        }
//        #endregion

//        #region Appointments
//        /// <summary>
//        /// Agenda una cita
//        /// </summary>
//        [Authorize(Policy = Permissions.GoogleCalendars.Schedule)]
//        [HttpPost("schedule")]
//        public async Task<IActionResult> ScheduleAppointment([FromBody] AppointmentSchedule appointment)
//        {
//            try
//            {
//                _logger.LogInformation("Intentando agendar cita para asesor {AdvisorId} y estudiante {StudentId}",
//                    appointment.AdvisorId, appointment.StudentId);

//                var result = await _googleCalendarService.ScheduleAppointmentAsync(appointment, appointment.AdvisorId);
//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al agendar cita: {Error}", ex.Message);
//                return BadRequest(new { Message = "Error al agendar cita", Error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Obtiene la disponibilidad del asesor
//        /// </summary>
//        [Authorize(Policy = Permissions.GoogleCalendars.Availability)]
//        [HttpGet("advisor-availability")]
//        public async Task<IActionResult> GetAdvisorAvailability(
//            [FromQuery] string advisorId,
//            [FromQuery] DateTime startDate,
//            [FromQuery] DateTime endDate)
//        {
//            try
//            {
//                _logger.LogInformation("Obteniendo disponibilidad para asesor {AdvisorId}", advisorId);
//                var availability = await _googleCalendarService.GetAdvisorAvailabilityAsync(advisorId, startDate, endDate, advisorId);
//                return Ok(availability);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener disponibilidad del asesor");
//                return BadRequest(new { Message = "Error al obtener disponibilidad", Error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Cancela una cita
//        /// </summary>
//        [Authorize(Policy = Permissions.GoogleCalendars.Cancel)]
//        [HttpPost("{appointmentId}/cancel")]
//        public async Task<IActionResult> CancelAppointment(
//            [FromRoute] string appointmentId,
//            [FromQuery] string reason,
//            [FromQuery] string userId)
//        {
//            try
//            {
//                var result = await _googleCalendarService.CancelAppointmentAsync(appointmentId, reason, userId);
//                return Ok(new { Success = result });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al cancelar cita");
//                return BadRequest(new { Message = "Error al cancelar cita", Error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Reprograma una cita
//        /// </summary>
//        [Authorize(Policy = Permissions.GoogleCalendars.Reschedule)]
//        [HttpPut("{appointmentId}/reschedule")]
//        public async Task<IActionResult> RescheduleAppointment(
//            [FromRoute] string appointmentId,
//            [FromBody] RescheduleRequest request)
//        {
//            try
//            {
//                var result = await _googleCalendarService.RescheduleAppointmentAsync(
//                    appointmentId,
//                    request.NewStartTime,
//                    request.NewEndTime,
//                    request.UserId);
//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al reprogramar cita");
//                return BadRequest(new { Message = "Error al reprogramar cita", Error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Obtiene las citas del usuario
//        /// </summary>
//        [Authorize(Policy = Permissions.GoogleCalendars.User)]
//        [HttpGet("user-appointments")]
//        public async Task<IActionResult> GetUserAppointments(
//            [FromQuery] string userId,
//            [FromQuery] string role,
//            [FromQuery] DateTime startDate,
//            [FromQuery] DateTime endDate)
//        {
//            try
//            {
//                var appointments = await _googleCalendarService.GetUserAppointmentsAsync(userId, role, startDate, endDate);
//                return Ok(appointments);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener citas del usuario");
//                return BadRequest(new { Message = "Error al obtener citas", Error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Obtiene los slots de tiempo disponibles
//        /// </summary>
//        [Authorize(Policy = Permissions.GoogleCalendars.Slots)]
//        [HttpGet("available-slots")]
//        public async Task<IActionResult> GetAvailableTimeSlots(
//            [FromQuery] string advisorId,
//            [FromQuery] DateTime date)
//        {
//            try
//            {
//                var slots = await _googleCalendarService.GetAvailableTimeSlotsAsync(advisorId, date, advisorId);
//                return Ok(slots);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener slots disponibles");
//                return BadRequest(new { Message = "Error al obtener slots", Error = ex.Message });
//            }
//        }
//        #endregion
//    }

//    public class RescheduleRequest
//    {
//        public DateTime NewStartTime { get; set; }
//        public DateTime NewEndTime { get; set; }
//        public string UserId { get; set; }
//    }
//}
