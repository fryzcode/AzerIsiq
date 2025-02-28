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
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiration { get; set; }
        public string? OtpCode { get; set; }
        public DateTime? OtpCodeExpiration { get; set; }
        // public string? UserAgent { get; set; }
        public string IpAddress { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<UserRole> UserRoles { get; set; } = new();
        
        public int FailedAttempts { get; set; } = 0;
        public DateTime? LockoutUntil { get; set; } 
    }
}
