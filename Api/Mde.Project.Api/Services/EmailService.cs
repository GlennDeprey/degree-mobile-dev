using MailKit.Net.Smtp;
using MailKit.Security;
using Mde.Project.Api.Services.Interfaces;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;

namespace Mde.Project.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendPasswordResetCodeAsync(string fullName, string email, string code)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:Account"]));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = "Stock Flow - Password Reset Code";

                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = $@"
                    <html>
                    <body>
                        <div style='font-family: Arial, sans-serif; color: #333;'>
                            <h2>Password Reset Code</h2>
                            <p>Dear {fullName},</p>
                            <p>Your password reset code is:</p>
                            <p style='font-size: 24px; font-weight: bold;'>{code}</p>
                            <p>If you did not request a password reset, please ignore this email.</p>
                            <p>Thank you,<br/>Stock Flow</p>
                        </div>
                    </body>
                    </html>"
                };

                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.CheckCertificateRevocation = false;
                    await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"],
                        int.Parse(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.SslOnConnect);
                    await smtp.AuthenticateAsync(_configuration["EmailSettings:Account"],
                        _configuration["EmailSettings:ApiKey"]);
                    await smtp.SendAsync(message);
                    await smtp.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
