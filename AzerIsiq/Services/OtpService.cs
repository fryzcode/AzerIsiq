using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using BCrypt.Net;

namespace AzerIsiq.Services;

public class OtpService
{
    private readonly IOtpCodeRepository _otpCodeRepository;

    public OtpService(IOtpCodeRepository otpCodeRepository)
    {
        _otpCodeRepository = otpCodeRepository;
    }
    public async Task<string> GenerateOtpAsync(int userId)
    {
        await _otpCodeRepository.DeleteExpiredOtpsByUserAsync(userId);

        if (!await CanRequestOtpAsync(userId))
            throw new UnauthorizedAccessException("OTP request limit reached for today.");
        
        var rawOtp = new Random().Next(100000, 999999).ToString();
        var hashedOtp = BCrypt.Net.BCrypt.HashPassword(rawOtp);

        var otp = new OtpCode
        {
            UserId = userId,
            Code = hashedOtp,
            Expiration = DateTime.UtcNow.AddMinutes(5),
            RequestCount = await _otpCodeRepository.GetOtpRequestCountAsync(userId) + 1,
            LastRequestTime = DateTime.UtcNow
        };

        await _otpCodeRepository.AddOtpAsync(otp);
        return rawOtp;
    }
    public async Task<bool> ValidateOtpAsync(int userId, string enteredOtp)
    {
        var latestOtp = await _otpCodeRepository.GetLatestOtpByUserIdAsync(userId);
        if (latestOtp == null || latestOtp.Expiration < DateTime.UtcNow)
            return false;

        return BCrypt.Net.BCrypt.Verify(enteredOtp, latestOtp.Code);
    }
    public async Task<bool> CanRequestOtpAsync(int userId)
    {
        var requestCount = await _otpCodeRepository.GetOtpRequestCountAsync(userId);
        return requestCount < 3;
    }
}