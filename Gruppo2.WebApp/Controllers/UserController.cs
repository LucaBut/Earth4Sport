using AutoMapper;
using Gruppo2.WebApp.Models;
using Gruppo2.WebApp.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gruppo2.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly WebAppContex _context;

        private readonly IMapper _mapper;
        public UserController(WebAppContex context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAsync()
        {
            List<User> users = new List<User>();
            try
            {
                users = await _context.User.ToListAsync();
            }
            catch (Exception ex) 
            {
                Console.Write(ex.ToString());
            }
            //users = await _context.User.ToListAsync();

            List<UserDto> usersDto = new List<UserDto>();
            _mapper.Map(users, usersDto);
            Console.WriteLine(users);
            return usersDto;
        }

        [HttpGet("GetUserByMail/{mail}")]
        public async Task<ActionResult<UserDto>> GetUserByEmailAsync(string mail)
        {
            User user = new User();
            try
            {
                user = await _context.User.Where(x => x.Email == mail).FirstAsync();
                UserDto dto = new UserDto();
                _mapper.Map(user, dto);
                return dto;
            }
            catch(Exception ex)
            {
                return NoContent();
            }

        }
    }
}
