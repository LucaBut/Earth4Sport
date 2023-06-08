namespace Gruppo2.AdminApp.ModelsDto
{
    public class NotificationErrorDto
    {
        public Guid Id { get; set; }
        public Guid IdActivity { get; set; }
        public Guid IdUser { get; set; }
        public string namesurnameUser { get; set; } = string.Empty;

        public string userName { get; set; } = string.Empty;

        public Guid IdDevice { get; set; }
        public string pulseRate { get; set; } = string.Empty;


        public DateTime Created { get; set; }
    }
}
