using AutoMapper;
using Gruppo2.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Gruppo2.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly WebAppContex _context;

        private readonly IMapper _mapper;
        public DeviceController(WebAppContex context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<bool>> GetDeviceAsync()
        {
            List<Device> devices = new List<Device>();
            devices = await _context.Device.ToListAsync();
            Console.WriteLine(devices);
            return true;
        }
    }
}
