namespace Gruppo2.WebApp.Models.Dtos
{
    public class UserDto
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? BirthDate { get; set; }
        public Guid? IdRole { get; set; }
    }
}
