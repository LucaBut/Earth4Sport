using Gruppo2.AdminApp.Models;
using Microsoft.EntityFrameworkCore;


namespace Gruppo2.AdminApp.Services

{
    public class AdminDBContext : DbContext
    {
        public AdminDBContext(DbContextOptions<AdminDBContext> options) : base(options)
        {

        }
        public virtual DbSet<NotificationError> NotificationError { get; set; }
    }
}
