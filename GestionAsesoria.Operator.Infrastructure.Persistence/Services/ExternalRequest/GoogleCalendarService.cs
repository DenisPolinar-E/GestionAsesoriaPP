//using GestionAsesoria.Operator.Application.Interfaces.Services.ExternalRequest;
//using GestionAsesoria.Operator.Domain.Entities;
//using GestionAsesoria.Operator.Infrastructure.Persistence.Configurations;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Calendar.v3;
//using Google.Apis.Calendar.v3.Data;
//using Google.Apis.Services;
//using Google.Apis.Util.Store;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace GestionAsesoria.Operator.Infrastructure.Persistence.Services.ExternalRequest
//{
//    public class GoogleCalendarService : IGoogleCalendarService
//    {
//        private readonly GoogleCalendarOptions _options;
//        private readonly ILogger<GoogleCalendarService> _logger;

//        public GoogleCalendarService(
//            IOptions<GoogleCalendarOptions> options,
//            ILogger<GoogleCalendarService> logger)
//        {
//            _options = options.Value;
//            _logger = logger;
//        }

//        private async Task<UserCredential> GetCredentialAsync(string userId)
//        {
//            if (string.IsNullOrEmpty(userId))
//            {
//                throw new ArgumentNullException(nameof(userId), "El userId no puede ser nulo o vacío.");
//            }

//            if (string.IsNullOrEmpty(_options.ClientId) || string.IsNullOrEmpty(_options.ClientSecret))
//            {
//                throw new InvalidOperationException("ClientId o ClientSecret no están configurados.");
//            }

//            if (_options.Scopes == null || !_options.Scopes.Any())
//            {
//                throw new InvalidOperationException("Scopes no están configurados.");
//            }

//            try
//            {
//                var credPath = Path.Combine("GoogleCalendarTokens", userId);
//                return await GoogleWebAuthorizationBroker.AuthorizeAsync(
//                    new ClientSecrets
//                    {
//                        ClientId = _options.ClientId,
//                        ClientSecret = _options.ClientSecret
//                    },
//                    _options.Scopes,
//                    userId,
//                    CancellationToken.None,
//                    new FileDataStore(credPath, true)
//                );
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error obteniendo credenciales");
//                throw;
//            }
//        }


//        private CalendarService CreateCalendarService(UserCredential credential)
//        {
//            return new CalendarService(new BaseClientService.Initializer()
//            {
//                HttpClientInitializer = credential,
//                ApplicationName = "AcademicAdvising"
//            });
//        }

//        public async Task<string> AuthorizeAsync(string userId)
//        {
//            var credential = await GetCredentialAsync(userId);
//            return credential.Token.AccessToken;
//        }

//        public async Task<GoogleCalendarEvent> CreateEventAsync(GoogleCalendarEvent calendarEvent, string userId)
//        {
//            var credential = await GetCredentialAsync(userId);
//            var service = CreateCalendarService(credential);

//            var googleEvent = new Event
//            {
//                Summary = calendarEvent.Summary,
//                Description = calendarEvent.Description,
//                Start = new EventDateTime
//                {
//                    DateTime = calendarEvent.StartTime,
//                    TimeZone = calendarEvent.TimeZone
//                },
//                End = new EventDateTime
//                {
//                    DateTime = calendarEvent.EndTime,
//                    TimeZone = calendarEvent.TimeZone
//                },
//                Location = calendarEvent.Location
//            };

//            var insertRequest = service.Events.Insert(googleEvent, "primary");
//            var insertedEvent = await insertRequest.ExecuteAsync();

//            return new GoogleCalendarEvent
//            {
//                Id = insertedEvent.Id,
//                Summary = insertedEvent.Summary,
//                Description = insertedEvent.Description,
//                StartTime = insertedEvent.Start.DateTime ?? DateTime.MinValue,
//                EndTime = insertedEvent.End.DateTime ?? DateTime.MinValue,
//                Location = insertedEvent.Location
//            };
//        }

//        public async Task<List<GoogleCalendarEvent>> GetEventsAsync(DateTime startDate, DateTime endDate, string userId)
//        {
//            var credential = await GetCredentialAsync(userId);
//            var service = CreateCalendarService(credential);

//            var request = service.Events.List("primary");
//            request.TimeMin = startDate;
//            request.TimeMax = endDate;
//            request.ShowDeleted = false;
//            request.SingleEvents = true;
//            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

//            var events = await request.ExecuteAsync();

//            return events.Items.Select(e => new GoogleCalendarEvent
//            {
//                Id = e.Id,
//                Summary = e.Summary,
//                Description = e.Description,
//                StartTime = e.Start.DateTime ?? DateTime.MinValue,
//                EndTime = e.End.DateTime ?? DateTime.MinValue,
//                Location = e.Location
//            }).ToList();
//        }

//        public async Task<bool> DeleteEventAsync(string eventId, string userId)
//        {
//            var credential = await GetCredentialAsync(userId);
//            var service = CreateCalendarService(credential);

//            await service.Events.Delete("primary", eventId).ExecuteAsync();
//            return true;
//        }

//        public async Task<GoogleCalendarEvent> UpdateEventAsync(GoogleCalendarEvent calendarEvent, string userId)
//        {
//            var credential = await GetCredentialAsync(userId);
//            var service = CreateCalendarService(credential);

//            var googleEvent = new Event
//            {
//                Id = calendarEvent.Id,
//                Summary = calendarEvent.Summary,
//                Description = calendarEvent.Description,
//                Start = new EventDateTime
//                {
//                    DateTime = calendarEvent.StartTime,
//                    TimeZone = calendarEvent.TimeZone
//                },
//                End = new EventDateTime
//                {
//                    DateTime = calendarEvent.EndTime,
//                    TimeZone = calendarEvent.TimeZone
//                },
//                Location = calendarEvent.Location
//            };

//            var updateRequest = service.Events.Update(googleEvent, "primary", calendarEvent.Id);
//            var updatedEvent = await updateRequest.ExecuteAsync();

//            return new GoogleCalendarEvent
//            {
//                Id = updatedEvent.Id,
//                Summary = updatedEvent.Summary,
//                Description = updatedEvent.Description,
//                StartTime = updatedEvent.Start.DateTime ?? DateTime.MinValue,
//                EndTime = updatedEvent.End.DateTime ?? DateTime.MinValue,
//                Location = updatedEvent.Location
//            };
//        }

//        public async Task<AppointmentSchedule> ScheduleAppointmentAsync(AppointmentSchedule appointment, string userId)
//        {
//            var credential = await GetCredentialAsync(userId);
//            var service = CreateCalendarService(credential);

//            var attendees = appointment.Attendees?.Select(email => new EventAttendee { Email = email }).ToList();

//            var googleEvent = new Event
//            {
//                Summary = $"Asesoría: {appointment.Title}",
//                Description = $"Asesor: {appointment.AdvisorId}\nEstudiante: {appointment.StudentId}\n\n{appointment.Description}",
//                Start = new EventDateTime
//                {
//                    DateTime = appointment.StartTime,
//                    TimeZone = appointment.TimeZone
//                },
//                End = new EventDateTime
//                {
//                    DateTime = appointment.EndTime,
//                    TimeZone = appointment.TimeZone
//                },
//                Location = appointment.Location,
//                Attendees = attendees,
//                ConferenceData = appointment.AppointmentType == "Virtual" ? new ConferenceData
//                {
//                    CreateRequest = new CreateConferenceRequest
//                    {
//                        RequestId = Guid.NewGuid().ToString(),
//                        ConferenceSolutionKey = new ConferenceSolutionKey { Type = "hangoutsMeet" }
//                    }
//                } : null
//            };

//            var insertRequest = service.Events.Insert(googleEvent, "primary");
//            insertRequest.ConferenceDataVersion = 1;
//            var insertedEvent = await insertRequest.ExecuteAsync();

//            appointment.Id = insertedEvent.Id;
//            appointment.MeetingLink = insertedEvent.HangoutLink;
//            appointment.CreatedAt = DateTime.UtcNow;
//            appointment.Status = "Pending";

//            return appointment;
//        }

//        public async Task<List<AppointmentSchedule>> GetAdvisorAvailabilityAsync(string advisorId, DateTime startDate, DateTime endDate, string userId)
//        {
//            var credential = await GetCredentialAsync(userId);
//            var service = CreateCalendarService(credential);

//            var request = service.Events.List("primary");
//            request.TimeMin = startDate;
//            request.TimeMax = endDate;
//            request.ShowDeleted = false;
//            request.SingleEvents = true;
//            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

//            var events = await request.ExecuteAsync();

//            return events.Items
//                .Where(e => e.Description?.Contains($"Asesor: {advisorId}") ?? false)
//                .Select(e => new AppointmentSchedule
//                {
//                    Id = e.Id,
//                    Title = e.Summary,
//                    Description = e.Description,
//                    StartTime = e.Start.DateTime ?? DateTime.MinValue,
//                    EndTime = e.End.DateTime ?? DateTime.MinValue,
//                    Location = e.Location,
//                    Status = e.Status,
//                    MeetingLink = e.HangoutLink,
//                    TimeZone = e.Start.TimeZone
//                }).ToList();
//        }

//        public async Task<bool> CancelAppointmentAsync(string appointmentId, string reason, string userId)
//        {
//            var credential = await GetCredentialAsync(userId);
//            var service = CreateCalendarService(credential);

//            var eventRequest = await service.Events.Get("primary", appointmentId).ExecuteAsync();
//            eventRequest.Status = "cancelled";
//            eventRequest.Description += $"\n\nCancelado: {reason}";

//            await service.Events.Update(eventRequest, "primary", appointmentId).ExecuteAsync();
//            return true;
//        }

//        public async Task<AppointmentSchedule> RescheduleAppointmentAsync(string appointmentId, DateTime newStartTime, DateTime newEndTime, string userId)
//        {
//            var credential = await GetCredentialAsync(userId);
//            var service = CreateCalendarService(credential);

//            var eventRequest = await service.Events.Get("primary", appointmentId).ExecuteAsync();

//            eventRequest.Start = new EventDateTime
//            {
//                DateTime = newStartTime,
//                TimeZone = eventRequest.Start.TimeZone
//            };

//            eventRequest.End = new EventDateTime
//            {
//                DateTime = newEndTime,
//                TimeZone = eventRequest.End.TimeZone
//            };

//            var updatedEvent = await service.Events.Update(eventRequest, "primary", appointmentId).ExecuteAsync();

//            return new AppointmentSchedule
//            {
//                Id = updatedEvent.Id,
//                Title = updatedEvent.Summary,
//                Description = updatedEvent.Description,
//                StartTime = updatedEvent.Start.DateTime ?? DateTime.MinValue,
//                EndTime = updatedEvent.End.DateTime ?? DateTime.MinValue,
//                Location = updatedEvent.Location,
//                Status = updatedEvent.Status,
//                MeetingLink = updatedEvent.HangoutLink,
//                TimeZone = updatedEvent.Start.TimeZone,
//                UpdatedAt = DateTime.UtcNow
//            };
//        }

//        public async Task<List<AppointmentSchedule>> GetUserAppointmentsAsync(string userId, string role, DateTime startDate, DateTime endDate)
//        {
//            var credential = await GetCredentialAsync(userId);
//            var service = CreateCalendarService(credential);

//            var request = service.Events.List("primary");
//            request.TimeMin = startDate;
//            request.TimeMax = endDate;
//            request.ShowDeleted = false;
//            request.SingleEvents = true;
//            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

//            var events = await request.ExecuteAsync();
//            var searchPattern = role == "Advisor" ? $"Asesor: {userId}" : $"Estudiante: {userId}";

//            return events.Items
//                .Where(e => e.Description?.Contains(searchPattern) ?? false)
//                .Select(e => new AppointmentSchedule
//                {
//                    Id = e.Id,
//                    Title = e.Summary,
//                    Description = e.Description,
//                    StartTime = e.Start.DateTime ?? DateTime.MinValue,
//                    EndTime = e.End.DateTime ?? DateTime.MinValue,
//                    Location = e.Location,
//                    Status = e.Status,
//                    MeetingLink = e.HangoutLink,
//                    TimeZone = e.Start.TimeZone,
//                    AppointmentType = !string.IsNullOrEmpty(e.HangoutLink) ? "Virtual" : "InPerson"
//                }).ToList();
//        }

//        public async Task<bool> ConfirmAppointmentAsync(string appointmentId, string userId)
//        {
//            var credential = await GetCredentialAsync(userId);
//            var service = CreateCalendarService(credential);

//            var eventRequest = await service.Events.Get("primary", appointmentId).ExecuteAsync();
//            eventRequest.Description += "\n\nEstado: Confirmado";

//            await service.Events.Update(eventRequest, "primary", appointmentId).ExecuteAsync();
//            return true;
//        }

//        public async Task<List<TimeSlot>> GetAvailableTimeSlotsAsync(string advisorId, DateTime date, string userId)
//        {
//            var credential = await GetCredentialAsync(userId);
//            var service = CreateCalendarService(credential);

//            // Obtener eventos del día
//            var request = service.Events.List("primary");
//            request.TimeMin = date.Date;
//            request.TimeMax = date.Date.AddDays(1).AddSeconds(-1);
//            request.ShowDeleted = false;
//            request.SingleEvents = true;
//            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

//            var events = await request.ExecuteAsync();

//            // Definir horario laboral (ejemplo: 7 AM a 11 PM)
//            var workDayStart = date.Date.AddHours(7);
//            var workDayEnd = date.Date.AddHours(23);

//            // Crear slots de 1 hora
//            var timeSlots = new List<TimeSlot>();
//            var currentSlotStart = workDayStart;

//            while (currentSlotStart < workDayEnd)
//            {
//                var slotEnd = currentSlotStart.AddHours(1);
//                var isAvailable = !events.Items.Any(e =>
//                    (e.Start.DateTime <= currentSlotStart && e.End.DateTime > currentSlotStart) ||
//                    (e.Start.DateTime < slotEnd && e.End.DateTime >= slotEnd));

//                timeSlots.Add(new TimeSlot
//                {
//                    StartTime = currentSlotStart,
//                    EndTime = slotEnd,
//                    IsAvailable = isAvailable
//                });

//                currentSlotStart = slotEnd;
//            }

//            return timeSlots;
//        }
//    }
//}
