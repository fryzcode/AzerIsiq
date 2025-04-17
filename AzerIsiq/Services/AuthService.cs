using System.Security.Claims;
using AzerIsiq.Dtos;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;

namespace AzerIsiq.Services;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly JwtService _jwtService;
    private readonly IEmailService _emailService;
    private readonly OtpService _otpService;
    public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, JwtService jwtService, IEmailService emailService, OtpService otpService, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _jwtService = jwtService;
        _emailService = emailService;
        _otpService = otpService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> RegisterAsync(RegisterDto dto, string ipAddress)
    {
        if (await _userRepository.ExistsByEmailAsync(dto.Email))
            throw new Exception("User with this email already exists");

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = hashedPassword,
            IsEmailVerified = false,
            IpAddress = ipAddress,
            CreatedAt = DateTime.UtcNow
        };

        user = await _userRepository.CreateAsync(user);

        await _userRepository.AddUserRoleAsync(user.Id, 1);

        user = await _userRepository.GetUserWithRolesAsync(user.Id);

        var token = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        await _userRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddMinutes(30));

        return token;
    }
    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, string ipAddress)
    {
        DateTime now = DateTime.UtcNow;
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password.");
        
        if (user.IsBlocked)
            throw new ForbiddenException("Your Account is blocked.");
        
        if (user.LastFailedAttempt.HasValue && user.FailedAttempts >= 3)
        {
            TimeSpan timeSinceLastFail = now - user.LastFailedAttempt.Value;

            if (user.FailedAttempts >= 5 && timeSinceLastFail.TotalMinutes < 30)
            {
                throw new UnauthorizedAccessException($"Account locked. Try again after {30 - (int)timeSinceLastFail.TotalMinutes} minutes.");
            }
            if (user.FailedAttempts >= 3 && timeSinceLastFail.TotalMinutes < 10)
            {
                throw new UnauthorizedAccessException($"Account locked. Try again after {10 - (int)timeSinceLastFail.TotalMinutes} minutes.");
            }
        }
        
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            if (user.LastFailedAttempt.HasValue && (now - user.LastFailedAttempt.Value).TotalMinutes >= 5)
            {
                user.FailedAttempts = 1;
            }
            else
            {
                user.FailedAttempts++;
            }

            user.LastFailedAttempt = now;
            await _userRepository.UpdateAsync(user);

            throw new UnauthorizedAccessException("Invalid email or password.");
        }
        
        await _userRepository.UpdateAsync(user);
        
        if (ipAddress != user.IpAddress)
        {
            if (!await _otpService.CanRequestOtpAsync(user.Id))
                throw new UnauthorizedAccessException("Too many OTP requests. Try later.");
            
            var otp = await _otpService.GenerateOtpAsync(user.Id);
            
            string subject = "OTP Code";
            string body = $@"
            <p>Hello, {user.UserName}!</p>
            <p>Your OTP code is: <strong>{otp}</strong></p>";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            throw new UnauthorizedAccessException("OTP sent to email. Please verify.");
        }
        
        return await GenerateAuthTokens(user);
    }
    public async Task<AuthResponseDto> VerifyOtpAsync(string email, string enteredOtp, string ipAddress)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid email.");
    
        bool isOtpValid = await _otpService.ValidateOtpAsync(user.Id, enteredOtp);
    
        if (!isOtpValid)
            throw new UnauthorizedAccessException("OTP is invalid or expired.");
    
        user.IpAddress = ipAddress;
        await _userRepository.UpdateAsync(user);

        return await GenerateAuthTokens(user);
    }
    private async Task<AuthResponseDto> GenerateAuthTokens(User user)
    {
        var token = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.FailedAttempts = 0;
        user.LastFailedAttempt = null;
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddMinutes(60));
        var roles = await _userRepository.GetUserRolesAsync(user.Id);

        return new AuthResponseDto
        {
            UserName = user.UserName,
            Email = user.Email,
            Token = token,
            RefreshToken = refreshToken,
            Roles = roles
        };
    }
    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null) return false;
    
        string resetToken = _jwtService.GenerateResetToken();
        DateTime tokenExpiry = DateTime.UtcNow.AddHours(1);
    
        await _userRepository.UpdateResetTokenAsync(user.Id, resetToken, tokenExpiry);
    
        string resetLink = $"http://localhost:3000/reset-password?t={resetToken}&email={dto.Email}";
    
        string subject = "Password Recovery";
        string body = $@"
        <p>Hello, {user.UserName}!</p>
        <p>You have requested to reset your password.</p>
        <p>Click the link below to reset it:</p>
        <p><a href='{resetLink}'>Reset Password</a></p>
        <p>If you did not request this, please ignore this email.</p>";

        await _emailService.SendEmailAsync(user.Email, subject, body);

        return true;
    }
    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _userRepository.GetByResetTokenAsync(dto.Token);
        
        if (user == null || user.Email != dto.Email)
        {
            throw new UnauthorizedAccessException("Invalid email or token.");
        }
        
        string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        
        await _userRepository.UpdatePasswordAsync(user.Id, newPasswordHash);

        return true;
    }
    public int GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User is not authorized.");
        }
        return int.Parse(userIdClaim.Value);
    }
}

