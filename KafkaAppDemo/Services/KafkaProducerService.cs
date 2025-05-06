using Confluent.Kafka;
using KafkaAppDemo.Models;
using Microsoft.Extensions.Options;

namespace KafkaAppDemo.Services
{
    public class KafkaProducerService
    {
        private readonly KafkaSettings _config;

        public KafkaProducerService(IOptions<KafkaSettings> config)
        {
            _config = config.Value;
        }

        public async Task<DeliveryResult<string, string>> ProduceAsync(KafkaMessage message)
        {
var producerConfig = new ConsumerConfig
            {
                BootstrapServers = _config.BootstrapServers,
                GroupId = _config.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
           

            using var producer = new ProducerBuilder<string, string>(producerConfig).Build();

            return await producer.ProduceAsync(message.Topic!, new Message<string, string>
            {
                Key = message.Key,
                Value = message.Value
            });
        }
    }
}
