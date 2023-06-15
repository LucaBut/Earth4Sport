using AutoMapper;
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
            List<Entities.Activity> activities = new List<Entities.Activity>();
            activities = await _context.Activity.ToListAsync();
            Console.WriteLine(activities);
            return true;
        }
        [HttpGet("GetActivitiesbyIDDevice/{idDeviceStr}")]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetActivitiesbyIDDevice(string idDeviceStr)
        {

            Guid idDevice = Guid.Parse(idDeviceStr);
            //put in list activity filtered by id device from db
            List<Entities.Activity> activities = new List<Entities.Activity>();
            activities = await _context.Activity
                                        .Where(x => x.IDDevice == idDevice)
                                        .ToListAsync();
            if (!activities.Any())
                return NoContent();

            
            List<ActivityDto> activityDtos= new List<ActivityDto>();

            foreach (Entities.Activity activity in activities)
            {
                ActivityDto activityDto = new ActivityDto();
                _mapper.Map(activity, activityDto);
                activityDtos.Add(activityDto);
            }
            //map activity list from db to a list to pass in frontend (dto)

            return activityDtos;
        }
    }
}
