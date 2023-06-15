namespace Gruppo2.WebApp.Entities
{
    public class ActivityContent
    {
        public Guid Id { get; set; }
        public Guid? IdActivity { get; set; }
        public string? Position { get; set; }
        public int? PulseRate { get; set; }
        public DateTime? Time { get; set; }

    }
}
