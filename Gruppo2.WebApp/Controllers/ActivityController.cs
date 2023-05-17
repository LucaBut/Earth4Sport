using AutoMapper;
using Gruppo2.WebApp.Models;
using Gruppo2.WebApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Gruppo2.WebApp.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly WebAppContex _context;

        private readonly IMapper _mapper;
        public ActivityController(WebAppContex context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<bool>> GetActivityAsync()
        {
            List<Models.Activity> activitys = new List<Models.Activity>();
            activitys = await _context.Activity.ToListAsync();
            Console.WriteLine(activitys);
            return true;
        }
        [HttpGet("GetActivitiesbyIDDevice")]
        public async Task<ActionResult<bool>> GetActivitiesbyIDDevice()
        {
            List<Models.Activity> activitys = new List<Models.Activity>();
            activitys = await _context.Activity.ToListAsync();
            Console.WriteLine(activitys);
            return true;
        }
    }
}
