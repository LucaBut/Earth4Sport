using Gruppo2.WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Gruppo2.WebApp
{
    public class WebAppContex: DbContext
    {
        public WebAppContex()
        {}

        public WebAppContex(DbContextOptions<WebAppContex> options): base(options)
        {

        }

        public virtual DbSet<User> User { get; set; }
    }
}
