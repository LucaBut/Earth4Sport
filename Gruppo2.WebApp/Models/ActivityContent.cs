namespace Gruppo2.WebApp.Models
{
    public class ActivityContent
    {
        public Guid Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StopDate { get; set; }
        public string? Position { get; set; }
        public int? pulseRate { get; set; }
        public Guid? IdActivity { get; set; }
    }
}
