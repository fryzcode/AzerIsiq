namespace AzerIsiq.Dtos
{
    public class RegisterDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PasswordConfirmation { get; set; }
        public string Role { get; set; } = "User";
    }
}
