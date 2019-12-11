using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Mail;
using FootballStatsApi.Web.Models;

namespace FootballStatsApi.Web.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly SmtpConnectionInfo _smtpConnectionInfo;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EmailSender(ILogger<EmailSender> logger, SmtpConnectionInfo smtpConnectionInfo, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _smtpConnectionInfo = smtpConnectionInfo;
            _hostingEnvironment = hostingEnvironment;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var client = new SmtpClient(_smtpConnectionInfo.Host, _smtpConnectionInfo.Port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpConnectionInfo.Username, _smtpConnectionInfo.Password);
                client.EnableSsl = true;

                var message = new MailMessage(_smtpConnectionInfo.FromAddress, email);
                message.IsBodyHtml = true;
                message.Body = htmlMessage;
                message.Subject = subject;

                client.Send(message);
            }
            
            return Task.CompletedTask;
        }
    }
}