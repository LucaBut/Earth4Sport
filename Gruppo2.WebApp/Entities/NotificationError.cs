namespace Gruppo2.WebApp.Entities
{
    public class NotificationError
    {
        public Guid Id { get; set; }
        public Guid IdActivity { get; set; }
        public Guid IdDevice { get; set; }
        public Guid IdUser { get; set; }
        public int PulseRate { get; set; }
        public DateTime Date { get; set; }
    }
}
