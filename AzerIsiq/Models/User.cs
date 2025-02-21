namespace AzerIsiq.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; } = false;
        public string? VerificationToken { get; set; }
        public string? ResetToken { get; set; }

        public List<UserRole> UserRoles { get; set; } = new();
    }
}
