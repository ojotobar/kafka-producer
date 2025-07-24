using KafkaAndQueueTest.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace KafkaAndQueueTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IKafkaProducer _kafkaProducer;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IKafkaProducer kafkaProducer)
        {
            _logger = logger;
            _kafkaProducer = kafkaProducer;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] PersonDto request)
        {
            if(request != null && !string.IsNullOrWhiteSpace(request.Name) && request.Age > 0)
            {
                await _kafkaProducer.ProduceAsync(nameof(Person), new Person
                {
                    Name = request.Name,
                    Age = request.Age
                });
                return Ok("Message sent!");
            }
            else
            {
                return BadRequest("Invalid request");
            }
        }
    }

    public class Person
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    public class PersonDto
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }


}
