using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly string smtpServer = "smtp.gmail.com"; // Gmail SMTP server
    private readonly int smtpPort = 587; // TLS port
    private readonly string smtpUser = "oahmed7241@gmail.com"; // Your Gmail address
    private readonly string smtpPassword = "tjhs vmbh noct ioan"; // The 16-character app password you generated

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var smtpClient = new SmtpClient(smtpServer)
        {
            Port = smtpPort,
            Credentials = new NetworkCredential(smtpUser, smtpPassword),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpUser),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        return smtpClient.SendMailAsync(mailMessage);
    }
}
