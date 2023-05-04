using Gruppo2.WebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gruppo2.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimulatorController : ControllerBase
    {
        private readonly SimulatorService _simulator;
        public SimulatorController(SimulatorService simulator)
        {
            _simulator = simulator;
        }


        [HttpGet("StartOperation")]
        public async Task StartOperation()
        {
            _simulator.Start();
             throw new Exception();
        }
    }
}
