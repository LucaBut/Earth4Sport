namespace Gruppo2.WebApp.Models.Dtos
{
    public class NotificationErrorDto
    {
        public Guid Id { get; set; }
        public Guid IdActivity { get; set; }
        public Guid IdDevice { get; set; }
        public string nameDevice { get; set; } = string.Empty;
        public Guid IdUser { get; set; }
        public string nameUser { get; set; } = string.Empty;
        public int PulseRate { get; set; }
        public DateTime Date { get; set; }
    }
}
