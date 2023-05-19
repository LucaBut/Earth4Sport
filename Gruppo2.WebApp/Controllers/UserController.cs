using AutoMapper;
using Gruppo2.WebApp.Models;
using Gruppo2.WebApp.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            users = await _context.User.ToListAsync();

            List<UserDto> usersDto = new List<UserDto>();
            _mapper.Map(users, usersDto);
            Console.WriteLine(users);
            return usersDto;
        }

        [HttpGet("{mail}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUserByEmailAsync(string mail)
        {
            List<User> user = new List<User>();
            user = await _context.User.Where(_ => _.Email == mail).ToListAsync();

            List<UserDto> userDtos = new List<UserDto>();
            foreach(User users in user) {
                UserDto userDto = new UserDto();
                _mapper.Map(users, userDto);
                userDtos.Add(userDto);
            }

            return userDtos;
        }
    }
}
