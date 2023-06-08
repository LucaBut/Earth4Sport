namespace Gruppo2.AdminApp.Models
{
    public class NotificationError
    {
        public Guid Id { get; set; }
        public Guid IdActivity { get; set; }
        public Guid IdDevice { get; set; }
        public string pulseRate { get; set; } =  string.Empty;

        public DateTime Created { get; set; }
    }
}
