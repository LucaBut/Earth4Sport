using AutoMapper;
using Gruppo2.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gruppo2.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly WebAppContex _context;

        private readonly IMapper _mapper;
        public RoleController(WebAppContex context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<bool>> GetRoleAsync()
        {
            List<Role> roles = new List<Role>();
            roles = await _context.Role.ToListAsync();
            Console.WriteLine(roles);
            return true;
        }
    }
}
