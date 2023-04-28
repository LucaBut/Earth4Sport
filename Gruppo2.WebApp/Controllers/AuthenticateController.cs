using AutoMapper;
using Gruppo2.WebApp.Models;
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

        [HttpGet("Login/{username}/{password}")]
        public async Task<ActionResult<bool>> Login(string username, string password)
        {
            
        }
    }
}
