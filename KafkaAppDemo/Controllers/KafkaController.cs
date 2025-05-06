using Microsoft.AspNetCore.Mvc;
using KafkaAppDemo.Services;
using KafkaAppDemo.Models;

namespace KafkaAppDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KafkaController : ControllerBase
{
   private readonly KafkaProducerService _producerService;
        private readonly ILogger _logger;

        public KafkaController(KafkaProducerService producerService, ILogger logger)
        {
            _producerService = producerService;
            _logger = logger;
        }

        [HttpPost("produce")]
        public async Task<IActionResult> Produce([FromBody] KafkaMessage message)
        {
            if (string.IsNullOrEmpty(message.Topic))
            {
                message.Topic = "demo-topic"; // Default topic if not specified
            }

            try
            {
                var result = await _producerService.ProduceAsync(message);
                
                _logger.LogInformation($"Message sent to topic {result.Topic}, partition {result.Partition}, offset {result.Offset}");
                
                return Ok(new 
                { 
                    Status = "Message sent successfully", 
                    Topic = result.Topic, 
                    Partition = result.Partition, 
                    Offset = result.Offset 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error producing message: {ex.Message}");
                return StatusCode(500, $"Failed to send message: {ex.Message}");
            }
        }
    }