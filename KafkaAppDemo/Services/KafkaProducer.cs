// using Confluent.Kafka;
// using Microsoft.Extensions.Options;
// using Confluent.Kafka.Admin;

// namespace KafkaAppDemo.Services;

// public class KafkaProducer
// {
//     private readonly IProducer<Null, string> _producer;
//     private readonly string _topic = "demo-topic1";

//     public KafkaProducer(IConfiguration config)
//     {
//         var producerConfig = new ProducerConfig
//         {
//             BootstrapServers = config["Kafka:BootstrapServers"]
//         };

//         _producer = new ProducerBuilder<Null, string>(producerConfig).Build();

//         using var adminClient = new AdminClientBuilder(producerConfig).Build();
//         try
//         {
//             adminClient.CreateTopicsAsync(new TopicSpecification[] {
//                 new TopicSpecification { Name = _topic, NumPartitions = 1, ReplicationFactor = 1 }
//             }).Wait();
//         }
//         catch (CreateTopicsException e) when (e.Results[0].Error.Code == ErrorCode.TopicAlreadyExists)
//         {
//             // Topic already exists - ignore
//         }
//     }

//     public async Task SendMessageAsync(string message)
//     {
//         await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
//     }
// }
