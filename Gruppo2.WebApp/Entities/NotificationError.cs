namespace Gruppo2.WebApp.Entities
{
    public class NotificationError
    {
        public int Id { get; set; }
        public Guid IdActivity { get; set; }
        public Guid IdDevice { get; set; }
        public string PulseRate { get; set; }
        public DateTime Created { get; set; }
    }
}
