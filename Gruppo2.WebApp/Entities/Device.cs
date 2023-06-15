namespace Gruppo2.WebApp.Entities
{
    public class Device
    {
        public Guid Id { get; set; }
        public string? ProductionBatch { get; set; }
        public string? Name { get; set; }
        public Guid? IDUser { get; set; }
    }
}
