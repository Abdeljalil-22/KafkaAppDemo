// using Confluent.Kafka;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using KafkaAppDemo.Models;

namespace KafkaAppDemo.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly ConsumerConfig  _config;
        private readonly string _topic;
        private readonly ILogger<KafkaConsumerService> _logger;

        public KafkaConsumerService(ConsumerConfig    config, ILogger<KafkaConsumerService>  logger)
        {
            _config = config;
            _topic = "demo-topic"; // Default topic to consume
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        }

        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            // var consumerConfig = new ConsumerConfig
            // {
            //     BootstrapServers = _config.BootstrapServers,
            //     GroupId = _config.GroupId,
            //     AutoOffsetReset = AutoOffsetReset.Earliest
            // };
            Console.WriteLine($"KafkaConsumerService: {_config.BootstrapServers} {_config.GroupId}");

            using var consumer = new ConsumerBuilder<Ignore, string>(_config)
                .SetErrorHandler((_, e) => _logger.LogError($"Error: {e.Reason}"))
                .Build();
            
            consumer.Subscribe(_topic);
            
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        
                        _logger.LogInformation($"Message received: {consumeResult.Message.Value} from topic: {consumeResult.Topic}");
                        
                        // Process the message here
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Consume error: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // The consumer was stopped
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}