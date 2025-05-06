using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace KafkaAppDemo.Services
{
    public class NotificationConsumerService : BackgroundService
    {
        private readonly ConsumerConfig _config;
        private readonly string _topic;
        private readonly ILogger _logger;

        public NotificationConsumerService(ConsumerConfig config, ILogger<NotificationConsumerService> logger)
        {
            _config = config;
            _topic = "notification-topic";
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        }

        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            using var consumer = new ConsumerBuilder<string, string>(_config)
            
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
                        
                        _logger.LogInformation($"Notification received: {consumeResult.Message.Value}");
                        
                        // Process notification here (e.g., send email, push notification)
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