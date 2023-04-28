namespace Gruppo2.WebApp.Models.Dtos
{
    public class ActivityDto
    {
        public Guid Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StopDate { get; set; }
        public int? PoolsNumber { get; set; }
        public Guid? IDDevice { get; set; }

    }
}
