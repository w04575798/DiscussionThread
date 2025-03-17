using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

public class EmailSender : IEmailSender  // 'public' should be before 'class EmailSender'
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Logic for sending email (you can simulate it or integrate actual email service here)
        return Task.CompletedTask;
    }
}
