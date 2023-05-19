namespace Gruppo2.WebApp.Models.Dtos
{
    public class ActivityContentDto
    {
        public Guid? Id{ get; set; }
        public Guid? IdActivity { get; set; }
        public string? PulseRate { get; set; }
        public string? Time { get; set; }
        public string? Position { get; set; }

    }
}
