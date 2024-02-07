using MediatR;
using N5.Microservices.User.Application.Commands;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.Infrastructure.Interfaces;
using N5.Utils;

namespace N5.Microservices.User.API.Services;
public class PermissionConsumerService : BackgroundService
{
    private readonly IEventConsumer<PermissionEventDto> _consumer;
    private readonly ILogger<PermissionConsumerService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public PermissionConsumerService(
        ILogger<PermissionConsumerService> logger, 
        IEventConsumer<PermissionEventDto> consumer,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _consumer = consumer;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("permissions");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _consumer.Consume(stoppingToken, async (permission, cct) =>
                {
                    await ProcessMessage(permission, cct);
                });
            }
            catch (Exception ex)
            {
                // TODO: Llevar a consumer
                _logger.LogError($"Error processing Kafka message: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

        _consumer.StopConsume();
    }

    public async Task ProcessMessage(PermissionEventDto permission, CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new SyncPermissionCommand(permission.EmployeeId), stoppingToken);
    }
}