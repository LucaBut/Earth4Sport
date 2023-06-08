using Gruppo2.AdminApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Gruppo2.AdminApp.Models;

namespace Gruppo2.AdminApp.Controllers
{       
    [ApiController]
    [Route("[controller]")]
    public class NotificationErrorController : ControllerBase
    {

        private readonly AdminDBContext _context;
        private readonly IMapper _mapper;
        public NotificationErrorController(AdminDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetNotificationErrors")]
        public async Task<ActionResult<IEnumerable<NotificationError>>> GetNotificationErrors()
        {
            List<NotificationError> errors = new List<NotificationError>();
            errors = await _context.NotificationError.ToListAsync();

            //aggiungere le chiamate al db esterno (link)
        }
    }
}
