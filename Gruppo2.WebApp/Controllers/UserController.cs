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
        public async Task<ActionResult<bool>> GetUsersAsync()
        {
            List<User> users = new List<User>();
            users = await _context.User.ToListAsync();
            Console.WriteLine(users);
            return true;
        }
    }
}
