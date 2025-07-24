using Confluent.Kafka;
using KafkaAndQueueTest.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace KafkaAndQueueTest.Services.Implementations
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly ILogger<KafkaProducer> _logger;
        private readonly IProducer<Null, string> _producer;

        public KafkaProducer(IConfiguration config, ILogger<KafkaProducer> logger)
        {
            _logger = logger;

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = config["Kafka:Server"]
            };

            _producer = new ProducerBuilder<Null, string>(producerConfig)
                .SetErrorHandler((_, e) => _logger.LogError($"Producer Error: {e.Reason}"))
                .Build();
        }

        public async Task ProduceAsync<TMessage>(string topic, TMessage value)
        {
            var messageJson = JsonSerializer.Serialize(value);

            var result = await _producer.ProduceAsync(topic, new Message<Null, string>
            {
                Value = messageJson
            });

            _logger.LogInformation($"Message delivered to {result.TopicPartitionOffset}");
        }
    }
}