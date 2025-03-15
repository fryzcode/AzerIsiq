namespace AzerIsiq.Services.ILogic;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string body);
}