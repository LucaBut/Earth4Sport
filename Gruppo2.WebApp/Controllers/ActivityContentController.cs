using AutoMapper;
using Gruppo2.WebApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Gruppo2.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityContentController : ControllerBase
    {
        private readonly WebAppContex _context;

        private readonly IMapper _mapper;
        public ActivityContentController(WebAppContex context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<bool>> GetActivityContentAsync()
        {
            List<ActivityContent> activityContents = new List<ActivityContent>();
            activityContents = await _context.ActivityContent.ToListAsync();
            Console.WriteLine(activityContents);
            return true;
        }
    }
}