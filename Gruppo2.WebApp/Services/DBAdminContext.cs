using Gruppo2.WebApp.Models;
using Microsoft.EntityFrameworkCore;


namespace Gruppo2.WebApp.Services
{
    public class DBAdminContext : DbContext
    {
        public DBAdminContext(DbContextOptions<DBAdminContext> optionsAdmin) : base(optionsAdmin)
        {}

        public virtual DbSet<NotificationError> NotificationError{ get; set; }
    }
}
