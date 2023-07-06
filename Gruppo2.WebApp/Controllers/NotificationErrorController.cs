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
            try
            {

                notificationsError = await _adminContext.NotificationError.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }


            if (!notificationsError.Any())
                return NoContent();

            List<NotificationErrorDto> errorsDto = new List<NotificationErrorDto>();



            List<Guid> idsUsers = new List<Guid>();
            idsUsers = notificationsError.Select(x => x.IdUser).ToList();

            List<Guid> idsDevices = new List<Guid>();
            idsDevices = notificationsError.Select(x => x.IdDevice).ToList();

            List<User> usersSelected = new List<User>();
            usersSelected = await _context.User.Where(x => idsUsers.Contains(x.Id)).ToListAsync();

            List<Device> devicesSelected = new List<Device>();
            devicesSelected = await _context.Device.Where(x => idsDevices.Contains(x.Id)).ToListAsync();



            foreach (NotificationError error in notificationsError)
            {
                User userSelected = new User();
                userSelected = usersSelected.First(x => x.Id == error.IdUser);

                NotificationErrorDto _error = new NotificationErrorDto();
                _error.nameUser = userSelected.Name + " " + userSelected.Surname;
                _error.nameDevice = devicesSelected.First(x => x.Id == error.IdDevice).Name;
                _mapper.Map(error, _error);
                errorsDto.Add(_error);
            }

            return errorsDto;
        }


    }

}
