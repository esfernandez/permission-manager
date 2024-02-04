using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using N5.Microservices.User.Infrastructure.Interfaces;
using N5.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Infrastructure;
public class KafkaProducer: IEventProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;

    public KafkaProducer(IConfiguration configuration, ILogger<KafkaProducer> logger)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration["ConnectionStrings:Kafka"]
        };

        _logger = logger;
        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    public async Task SendEvent(string topic, object msg)
    {
        var result = await _producer.ProduceAsync(topic, new Message<string, string> { Value = msg.ToJson() });
        _logger.Log(LogLevel.Debug, $"Delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }

}
