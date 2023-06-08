using Gruppo2.AdminApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Gruppo2.AdminApp.Models;
using Gruppo2.AdminApp.ModelsDto;

namespace Gruppo2.AdminApp.Controllers
{       
    [ApiController]
    [Route("[controller]")]
    public class NotificationErrorController : ControllerBase
    {

        private readonly AdminDBContext _context;
        private readonly IMapper _mapper;
        private readonly HttpClient _client;
        public NotificationErrorController(AdminDBContext context, IMapper mapper, HttpClient client)
        {
            _context = context;
            _mapper = mapper;
            _client = client;   

        }

        [HttpGet("GetNotificationErrors")]
        public async Task<ActionResult<IEnumerable<NotificationErrorDto>>> GetNotificationErrors()
        {
            List<NotificationError> errors = new List<NotificationError>();
            errors = await _context.NotificationError.ToListAsync();
            if (!errors.Any())
                return NoContent();

            List<NotificationErrorDto> errorsDto = new List<NotificationErrorDto>();

            HttpResponseMessage response = await _client.GetAsync("{api da mettere pubbliche}");
            //aggiungere le chiamate al db esterno (link)
            foreach (NotificationError error in errors)
            {

            }

            return errorsDto;

        }
    }
}
