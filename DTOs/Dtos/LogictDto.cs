namespace DTOs.Dtos
{
    public class LogistDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public LogistRole Role { get; set; }
        public bool IsExpired { get; set; }
        public PasswordState PasswordState { get; set; }
    }
}
