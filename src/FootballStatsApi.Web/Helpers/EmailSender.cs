using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace FootballStatsApi.Web.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            this._logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            this._logger.LogInformation($"Sending email to {email} with subject {subject} and body {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}