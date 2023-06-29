using System.ComponentModel.DataAnnotations;

namespace Gruppo2.WebApp.Entities
{
    public class Activity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StopDate { get; set; }
        public int? PoolsNumber { get; set; }
        public Guid? IDDevice { get; set; }

    }
}
