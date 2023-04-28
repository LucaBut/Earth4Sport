namespace Gruppo2.WebApp.Models.Dtos
{
    public class DeviceDto
    {
        public Guid Id { get; set; }
        public string? ProductionBatch { get; set; }
        public string? Name { get; set; }
        public Guid? IDUser { get; set; }
    }
}
