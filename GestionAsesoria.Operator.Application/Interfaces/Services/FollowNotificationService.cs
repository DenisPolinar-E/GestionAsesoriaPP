using GestionAsesoria.Operator.Application.DTOs.Follow.Request;
using GestionAsesoria.Operator.Application.DTOs.Follow.Response;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.Models;

public class FollowNotificationService
{
    private readonly IFollowRepositoryAsync _followRepository;
    private readonly INotificationService _notificationService;
    private readonly string _commissionEmail;
    private readonly ILogger<FollowNotificationService> _logger;

    public FollowNotificationService(
        IFollowRepositoryAsync followRepository,
        INotificationService notificationService,
        IOptions<NotificationSettings> settings,
        ILogger<FollowNotificationService> logger)
    {
        _followRepository = followRepository;
        _notificationService = notificationService;
        _commissionEmail = settings.Value.CommissionEmail;
        _logger = logger;
    }

    public async Task NotifyExpiringInternshipsAsync()
    {
        var expiring = await _followRepository.GetInternshipsExpiringIn7DaysAsync();

        foreach (var item in expiring)
        {
            if (string.IsNullOrWhiteSpace(item.StudentEmail))
            {
                _logger.LogWarning($"No se puede enviar notificación al estudiante {item.StudentName}: correo inválido.");
                continue;
            }

            var studentMsg = $"Hola {item.StudentName}, tu práctica finaliza en 7 días ({item.EndPreProfessionalPractice:dd/MM/yyyy}). Por favor, coordina con tu asesor.";
            var advisorMsg = $"Hola {item.AdvisorName}, el estudiante {item.StudentName} finaliza su práctica en 7 días ({item.EndPreProfessionalPractice:dd/MM/yyyy}).";
            var commissionMsg = $"El estudiante {item.StudentName} está por finalizar su práctica profesional el {item.EndPreProfessionalPractice:dd/MM/yyyy}.";

            await _notificationService.NotifyByEmailAsync(item.StudentEmail, "Práctica por finalizar", studentMsg);

            if (!string.IsNullOrWhiteSpace(item.AdvisorEmail))
            {
                await _notificationService.NotifyByEmailAsync(item.AdvisorEmail, "Seguimiento de práctica", advisorMsg);
            }
            else
            {
                _logger.LogWarning($"No se puede enviar notificación al asesor de {item.StudentName}: correo inválido.");
            }

            if (!string.IsNullOrWhiteSpace(_commissionEmail))
            {
                await _notificationService.NotifyByEmailAsync(_commissionEmail, "Alerta de práctica próxima a finalizar", commissionMsg);
            }
            else
            {
                _logger.LogWarning($"No se puede enviar notificación a la comisión para {item.StudentName}: correo de comisión inválido.");
            }
        }
    }

    public async Task NotifyImmediateAlertAsync(ListFollowDto item)
    {
        if (string.IsNullOrWhiteSpace(item.StudentEmail))
        {
            _logger.LogWarning($"No se puede enviar notificación inmediata al estudiante {item.StudentName}: correo inválido.");
            return;
        }

        var studentMsg = $"Hola {item.StudentName}, tu práctica ha sido registrada o modificada.";
        var advisorMsg = $"Hola {item.AdvisorName}, el estudiante {item.StudentName} tiene cambios en su práctica.";
        var commissionMsg = $"Actualización sobre el estudiante {item.StudentName}.";

        await _notificationService.NotifyByEmailAsync(item.StudentEmail, "Notificación inmediata práctica", studentMsg);

        if (!string.IsNullOrWhiteSpace(item.AdvisorEmail))
        {
            await _notificationService.NotifyByEmailAsync(item.AdvisorEmail, "Notificación inmediata práctica", advisorMsg);
        }

        if (!string.IsNullOrWhiteSpace(_commissionEmail))
        {
            await _notificationService.NotifyByEmailAsync(_commissionEmail, "Notificación inmediata práctica", commissionMsg);
        }
    }
}