using AzerIsiq.Dtos;

namespace AzerIsiq.Services.ILogic;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto dto, string ipAddress);
    Task<AuthResponseDto> LoginAsync(LoginDto dto, string ipAddress);
    Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto dto);

    Task<AuthResponseDto> VerifyOtpAsync(string email, string enteredOtp, string ipAddress);
    public int GetCurrentUserId();
    // Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto);
    //Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto);
}