using AzerIsiq.Dtos;

namespace AzerIsiq.Services;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    // Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto);
    //Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto);
}