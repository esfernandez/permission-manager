using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure;
using N5.Microservices.User.Infrastructure.Interfaces;
using N5.Utils;
using static Confluent.Kafka.ConfigPropertyNames;

public class KafkaConsumerService<TValue>: IEventConsumer<TValue> where TValue : class
{
    private readonly IConsumer<string, string> _consumer;
    private readonly ILogger<KafkaConsumerService<TValue>> _logger;
    private readonly IEventProducer _producer;

    public KafkaConsumerService(IOptions<KafkaOptions> options, ILogger<KafkaConsumerService<TValue>> logger, IEventProducer producer)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = options.Value.Url,
            GroupId = "UserMicroserviceGroup",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            
        };
        _logger = logger;
        _producer = producer;
        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
    }

    public void ProcessKafkaMessage(CancellationToken stoppingToken)
    {
        try
        {
            var consumeResult = _consumer.Consume(stoppingToken);

            var message = consumeResult.Message.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing Kafka message: {ex.Message}");
        }
    }

    public void Subscribe(string key)
    {
        _consumer.Subscribe(key);
    }

    public void StopConsume()
    {
        _consumer.Close();
    }

    public async Task Consume(CancellationToken cancellationToken, Func<TValue, CancellationToken, Task> action)
    {
        var consumeResult = _consumer.Consume(cancellationToken);
        try
        {  
            if (consumeResult != null && !consumeResult.IsPartitionEOF)
            {
                await action(consumeResult.Message.Value.FromJsonToObject<TValue>(), cancellationToken);
                _logger.LogInformation($"Received {consumeResult.Message.Key}: {consumeResult.Message.Value}");
            }
            _consumer.Commit(consumeResult);
        }
        catch(Exception ex)
        {
            await _producer.SendEvent(consumeResult.Message.Key + "error", consumeResult.Message.Value);
            throw;
        }
        finally
        {
            _consumer.Commit(consumeResult);
        }
    }
}
