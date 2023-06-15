using Gruppo2.WebApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gruppo2.WebApp
{
    public class WebAppContex: DbContext
    {
        public WebAppContex(DbContextOptions<WebAppContex> options): base(options)
        {

        }

        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<ActivityContent> ActivityContent { get; set; }
        public virtual DbSet<Device> Device { get; set; }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<NotificationError> NotificationError { get; set; }
    }
}
