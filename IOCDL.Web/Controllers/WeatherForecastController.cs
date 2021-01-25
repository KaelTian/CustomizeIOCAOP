using IOCDL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IOCDL.Web.Controllers
{
    [ApiController]
    [Route("weatherforecast")]
    //[AllowAnonymous]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ITestServiceA _testServiceA;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITestServiceA testServiceA)
        {
            _logger = logger;
            _testServiceA = testServiceA;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            //_testServiceA.ShowMessage111("LOL HAHAHAHAH");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                //Summary = Summaries[rng.Next(Summaries.Length)]
                Summary = _testServiceA.ShowMessage111("LOL HAHAHAHAH")
            })
            .ToArray();
        }
    }
}
