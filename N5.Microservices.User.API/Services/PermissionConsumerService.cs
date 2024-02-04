using MediatR;
using N5.Microservices.User.Application.Commands;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.Infrastructure.Interfaces;
using N5.Utils;

namespace N5.Microservices.User.API.Services;
public class PermissionConsumerService : BackgroundService
{
    private readonly IEventConsumer<PermissionDto> _consumer;
    private readonly IMediator _mediator;
    private readonly ILogger<PermissionConsumerService> _logger;

    public PermissionConsumerService(ILogger<PermissionConsumerService> logger, IEventConsumer<PermissionDto> consumer, IMediator mediator)
    {
        _logger = logger;
        _consumer = consumer;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("Permissions.Updates");

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

    public async Task ProcessMessage(PermissionDto permission, CancellationToken stoppingToken)
    {
        await _mediator.Send(new SyncPermissionCommand(permission.Id), stoppingToken);
    }
}