using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface IOtpCodeRepository : IGenericRepository<OtpCode>
{
    Task<OtpCode> GetLatestOtpByUserIdAsync(int userId);
    Task<int> GetOtpRequestCountAsync(int userId);
    Task AddOtpAsync(OtpCode otp);
    Task DeleteExpiredOtpsByUserAsync(int userId);
}