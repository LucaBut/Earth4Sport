using Gruppo2.WebApp.Entities;
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
        DbContext context;
        public UserController(DbContext context)
        {
            
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var res = await context.FindAsync<User>();
            context.SaveChangesAsync();
            return (IEnumerable<User>)res;
        }
    }
}
