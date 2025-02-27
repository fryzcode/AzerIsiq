using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly JwtService _jwtService;
    private readonly IEmailService _emailService;

    public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, JwtService jwtService, IEmailService emailService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _jwtService = jwtService;
        _emailService = emailService;
    }

    public async Task<string> RegisterAsync(RegisterDto dto)
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
            IsEmailVerified = false
        };

        user = await _userRepository.CreateAsync(user);

        await _userRepository.AddUserRoleAsync(user.Id, 1);

        user = await _userRepository.GetUserWithRolesAsync(user.Id);

        var token = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        await _userRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddMinutes(30));

        return token;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception("Invalid credentials");

        var token = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddMinutes(30));

        return new AuthResponseDto
        {
            UserName = user.UserName,
            Email = user.Email,
            Token = token,
            RefreshToken = user.RefreshToken
        };
    }
    
    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null) return false;
    
        string resetToken = _jwtService.GenerateResetToken();
        DateTime tokenExpiry = DateTime.UtcNow.AddHours(1);
    
        await _userRepository.UpdateResetTokenAsync(user.Id, resetToken, tokenExpiry);
    
        string resetLink = $"http://localhost:5297/api/auth/reset-password?token={resetToken}";
    
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
            return false;
        }
        
        string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        
        await _userRepository.UpdatePasswordAsync(user.Id, newPasswordHash);

        return true;
    }

}

