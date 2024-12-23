using System.Net;
using System.Net.Mail;

namespace MarketPlace.Data.Services
{
    public class EmailService : IEmailService
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPass;
    
    public EmailService(IConfiguration configuration)
    {
        _smtpHost = configuration["Email:SmtpHost"];
        _smtpPort = int.Parse(configuration["Email:SmtpPort"]);
        _smtpUser = configuration["Email:SmtpUser"];
        _smtpPass = configuration["Email:SmtpPass"];
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string message)
    {
        try
        {
            var MailMessage = new MailMessage{
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            MailMessage.To.Add(toEmail);

            using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                await smtpClient.SendMailAsync(MailMessage);
            }
            return true;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }
}
}