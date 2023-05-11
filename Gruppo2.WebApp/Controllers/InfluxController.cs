using Gruppo2.WebApp.Models;
using InfluxDB.Client;
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
        public Task Invoke() 
        {
                Guid idActivity = Guid.NewGuid();
                string idActivityStr = idActivity.ToString();
                string position = "654654646546 ,67657657657";
                string pulseRate = "192";    

                string record = idActivityStr + ";" + position + ";" + pulseRate;
                _InfluxDbService.Write(record);
               
            return Task.CompletedTask;
        }






    }
}
