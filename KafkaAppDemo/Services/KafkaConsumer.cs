// using Confluent.Kafka;

// namespace KafkaAppDemo.Services;

// public class KafkaConsumer : BackgroundService
// {
//     private readonly IConfiguration _config;

//     public KafkaConsumer(IConfiguration config)
//     {
//         _config = config;
//     }
// protected override async Task ExecuteAsync(CancellationToken stoppingToken)
// {
//     try
//     {
//         _consumer.Subscribe("demo-topic");

//         while (!stoppingToken.IsCancellationRequested)
//         {
//             var result = _consumer.Consume(stoppingToken);
//             Console.WriteLine($"Consumed: {result.Message.Value}");
//         }
//     }
//     catch (OperationCanceledException)
//     {
//         // Expected on shutdown, do nothing or log nicely
//         Console.WriteLine("Kafka consumer stopping...");
//     }
//     finally
//     {
//         _consumer.Close();
//     }
// }
//     // protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     // {
//     //     var consumerConfig = new ConsumerConfig
//     //     {
//     //         BootstrapServers = _config["Kafka:BootstrapServers"],
//     //         GroupId = "demo-consumer-group",
//     //         AutoOffsetReset = AutoOffsetReset.Earliest
//     //     };

//     //     using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
//     //     consumer.Subscribe("demo-topic");

//     //     while (!stoppingToken.IsCancellationRequested)
//     //     {
//     //         var result = consumer.Consume(stoppingToken);
//     //         Console.WriteLine($"Consumed: {result.Message.Value}");
//     //     }
//     // }
// }
