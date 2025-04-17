using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzerIsiq.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } =  null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool IsEmailVerified { get; set; } = false; 
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }
        [MaxLength(1000)]
        public string? ResetToken { get; set; } 
        public DateTime? ResetTokenExpiration { get; set; }
        [MaxLength(50)]
        public string IpAddress { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<UserRole> UserRoles { get; set; } = new();
    
        public int FailedAttempts { get; set; } = 0; 
        public DateTime? LastFailedAttempt { get; set; } 
        public List<OtpCode> OtpCodes { get; set; } = new();
        public bool IsBlocked { get; set; } = false;
    }

}