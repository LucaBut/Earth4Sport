using AutoMapper;
using Gruppo2.WebApp.Models;
using Gruppo2.WebApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Gruppo2.WebApp.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly WebAppContex _context;

        private readonly IMapper _mapper;
        public AuthenticateController(WebAppContex context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Login/{username}/{password}")]//prova login, da cambiare con auth0
        public async Task<ActionResult<bool>> Login(string username, string password)
        {
            User user = new User();
            user = await _context.User.Where(x => x.Username == username && x.Password == password).FirstAsync();
            if(user.Id == null) //non trovo nessun user con quelle credenziali
            {
                return NoContent();
            }
            UserDto userDto = new UserDto();
            _mapper.Map(user, userDto);

            return Ok(userDto);
        }
    }
}
