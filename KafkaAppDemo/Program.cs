using KafkaAppDemo.Services;
using KafkaAppDemo.Models;
// using Microsoft.Extensions.Options;
using Confluent.Kafka;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Kafka Producer
// builder.Services.Configure(builder.Configuration.GetSection("KafkaProducer"));
builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<KafkaProducerService>();

// var consumerConfig = builder.Configuration.GetSection("KafkaConsumer").Get<ConsumerConfig>();
// consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
// consumerConfig.EnableAutoCommit = true;
// builder.Services.AddSingleton(consumerConfig);


// builder.Services.Configure<ConsumerConfig>(config =>
// {
//     builder.Configuration.GetSection("KafkaConsumer").Bind(config);
//     config.AutoOffsetReset = AutoOffsetReset.Earliest;
//     config.EnableAutoCommit = true;
// });

// builder.Services.Configure(options =>
// {
//     builder.Configuration.GetSection("KafkaConsumer").Bind(options);
//     options.AutoOffsetReset = AutoOffsetReset.Earliest;
//     options.EnableAutoCommit = true;
// });
// Configure Kafka Consumer
// builder.Services.Configure(options =>
// {
//     builder.Configuration.GetSection("KafkaConsumer").Bind(options);
//     options.AutoOffsetReset = AutoOffsetReset.Earliest;
//     options.EnableAutoCommit = true;     
// });
// builder.Services.AddHostedService<KafkaConsumerService>();
builder.Services.AddSingleton(sp => {
    var cfg = new ConsumerConfig();
    builder.Configuration.GetSection("KafkaConsumer").Bind(cfg);
    Console.WriteLine($"KafkaConsumerService: {cfg.BootstrapServers} {cfg.GroupId}");
    return cfg;
});

builder.Services.AddHostedService<KafkaConsumerService>();
builder.Services.AddHostedService<NotificationConsumerService>();

// builder.Services.AddSingleton<KafkaProducer>();
// builder.Services.AddHostedService<KafkaConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
