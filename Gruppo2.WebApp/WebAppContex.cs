using Gruppo2.WebApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gruppo2.WebApp
{
    public class WebAppContex: DbContext
    {
        DbContext context;

        public WebAppContex()
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}
