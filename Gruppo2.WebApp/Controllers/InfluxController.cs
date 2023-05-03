using Gruppo2.WebApp.Models;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.CompilerServices;

namespace Gruppo2.WebApp.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class InfluxController : ControllerBase
    {
        private readonly InfluxDBService _InfluxDbService;
        private static readonly Random _random = new Random();

        public InfluxController(InfluxDBService influxDbService)
        {

            _InfluxDbService = influxDbService;

        }

        [HttpGet]
        public Task Invoke() //Non funzionante
        {
            _InfluxDbService.Write(write =>
            {
                var point = PointData.Measurement("altitude")
                    .Tag("plane", "test-plane")
                    .Field("value", _random.Next(1000, 5000))
                    .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

                write.WritePoint(point, "test", "ciao");
            });

            return Task.CompletedTask;
        }
    }
}
