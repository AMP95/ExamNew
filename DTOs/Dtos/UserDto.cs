namespace DTOs.Dtos
{
    public class UserDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public bool IsExpired { get; set; }
        public PasswordState PasswordState { get; set; }
    }
}
