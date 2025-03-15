using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AzerIsiq.Services;
using AzerIsiq.Services.ILogic;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SendEmailAsync(string email, string subject, string body)
    {
        try
        {
            string smtpServer = _configuration["Smtp:Server"];
            int smtpPort = int.Parse(_configuration["Smtp:Port"]);
            string smtpUser = _configuration["Smtp:User"];
            string smtpPass = _configuration["Smtp:Password"];
            string senderEmail = _configuration["Smtp:SenderEmail"];

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            using var message = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(email);

            await client.SendMailAsync(message);
            Console.WriteLine("✅ Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error sending email: {ex.Message}");
        }
    }
}