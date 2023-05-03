using Gruppo2.WebApp.Models;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.AspNetCore.Mvc;
using System;

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

        [HttpPost]
        public Task Invoke()
        {
            //_InfluxDbService.Write(write =>
            //{
            //    var point = PointData.Measurement("altitude")
            //        .Tag("plane", "test-plane")
            //        .Field("value", _random.Next(1000, 5000))
            //        .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            //    write.WritePoint("test-bucket", "organization", point);
            //});

            return Task.CompletedTask;
        }
    }
}
