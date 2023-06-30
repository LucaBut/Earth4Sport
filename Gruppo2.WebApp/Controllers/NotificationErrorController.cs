using AutoMapper;
using Gruppo2.WebApp.Entities;
using Gruppo2.WebApp.Models.Dtos;
using Gruppo2.WebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gruppo2.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationErrorController : ControllerBase
    {
        private readonly WebAppContex _context;
        private readonly DBAdminContext _adminContext;
        private readonly IMapper _mapper;

        public NotificationErrorController(WebAppContex context, DBAdminContext adminContext, IMapper mapper)
        {
            _context = context;
            _adminContext = adminContext;
            _mapper = mapper;
        }

        [HttpGet("GetNotificationsErrors")]
        public async Task<ActionResult<IEnumerable<NotificationErrorDto>>> GetNotificationsErrors()
        {
            List<NotificationError> notificationsError = new List<NotificationError>();
            notificationsError = await _adminContext.NotificationError.ToListAsync();

            if (!notificationsError.Any())
                return NoContent();

            List<NotificationErrorDto> errorsDto = new List<NotificationErrorDto>();

            foreach(NotificationError error in notificationsError)
            {
                NotificationErrorDto _error = new NotificationErrorDto();
                _mapper.Map(error, _error);
                errorsDto.Add(_error);
            }

            return errorsDto;
        }


    }

}
