using GUP.Ecommerce.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace GUP.Ecommerce.Services;

public class EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger) : IEmailSender
{
    private readonly MailSettings _mailSettings = mailSettings.Value;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        MailMessage message = new()
        {
            From = new MailAddress(_mailSettings.Mail!, _mailSettings.DisplayName),
            Body = htmlMessage,
            Subject = subject,
            IsBodyHtml = true
        };

        message.To.Add(email);

        using System.Net.Mail.SmtpClient smtpClient = new(_mailSettings.Host)
        {
            Port = _mailSettings.Port,
            Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password),
            EnableSsl = true
        };

        await smtpClient.SendMailAsync(message);

        smtpClient.Dispose();
    }

    //public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    //{
    //    var message = new MimeMessage
    //    {
    //        Sender = MailboxAddress.Parse(_mailSettings.Mail),
    //        Subject = subject
    //    };

    //    message.To.Add(MailboxAddress.Parse(email));

    //    var builder = new BodyBuilder
    //    {
    //        HtmlBody = htmlMessage
    //    };

    //    message.Body = builder.ToMessageBody();

    //    using var smtp = new SmtpClient();

    //    _logger.LogInformation("Sending email to {email}", email);

    //    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
    //    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
    //    await smtp.SendAsync(message);
    //    smtp.Disconnect(true);
    //}
}