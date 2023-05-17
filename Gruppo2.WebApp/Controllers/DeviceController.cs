using AutoMapper;
using Gruppo2.WebApp.Models;
using Gruppo2.WebApp.Models.Dtos;
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


        [HttpGet("GetDevicesbyIDUser/{idUserStr}")]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetDevicesbyIDUser(string idUserStr)
        {
            Guid idUser = Guid.Parse(idUserStr);

            List<Device> devices = new List<Device>();

            //recupero devices filtrati per idUser
            devices = await _context.Device
                                    .Where(x => x.IDUser == idUser)
                                    .ToListAsync();
            if (!devices.Any())
                return NoContent();
            
            List<DeviceDto> deviceDtos = new List<DeviceDto>();
            //questo serve per mappare i Models con i ModelsDto
            foreach(Device device in devices) 
            { 
                DeviceDto dto = new DeviceDto();
                _mapper.Map(device, dto);
                deviceDtos.Add(dto);
            }

            return deviceDtos;
        }

    }
}
