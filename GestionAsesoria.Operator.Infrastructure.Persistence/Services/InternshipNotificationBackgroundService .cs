using System;
using System.Threading;
using System.Threading.Tasks;
using GestionAsesoria.Operator.Application.Interfaces.Repositories;
using GestionAsesoria.Operator.Application.Interfaces.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class InternshipNotificationBackgroundService : BackgroundService
{
    private readonly ILogger<InternshipNotificationBackgroundService> _logger;
    private readonly FollowNotificationService _notificationService;
    private readonly IFollowRepositoryAsync _followRepository;

    public InternshipNotificationBackgroundService(
        ILogger<InternshipNotificationBackgroundService> logger,
        FollowNotificationService notificationService,
        IFollowRepositoryAsync followRepository)
    {
        _logger = logger;
        _notificationService = notificationService;
        _followRepository = followRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio de notificación iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Ejecutando chequeo de prácticas por vencer...");
                await _notificationService.NotifyExpiringInternshipsAsync();
                await _followRepository.UpdateExpiredInternshipsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando notificaciones o actualización de estados.");
            }

            var now = DateTime.Now;
            var nextRunTime = DateTime.Today.AddDays(1).AddHours(7);
            var delay = nextRunTime - now;

            if (delay.TotalMilliseconds <= 0)
            {
                delay = TimeSpan.FromHours(24);
            }

            _logger.LogInformation($"Próxima ejecución programada en {delay.TotalHours} horas.");
            await Task.Delay(delay, stoppingToken);
        }

        _logger.LogInformation("Servicio de notificación detenido.");
    }
}