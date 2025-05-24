using System.Net.Mail;
using System.Net;

namespace TechGalaxyProject.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public SmtpEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            try
            {
                Console.WriteLine("📨 Preparing to send email...");

                var smtpClient = new SmtpClient
                {
                    Host = _config["Smtp:Host"],
                    Port = int.Parse(_config["Smtp:Port"]!),
                    EnableSsl = true,
                    Credentials = new NetworkCredential(
                        _config["Smtp:Username"],
                        _config["Smtp:Password"]
                    )
                };

                var mail = new MailMessage
                {
                    From = new MailAddress(_config["Smtp:SenderEmail"]!, _config["Smtp:SenderName"]),
                    Subject = subject,
                    IsBodyHtml = true
                };

                mail.To.Add(toEmail);
                mail.ReplyToList.Add(new MailAddress(_config["Smtp:SenderEmail"]!));
                mail.Headers.Add("X-Priority", "1"); // optional priority flag

                // Add both plain text and HTML versions for better delivery
                var plainText = "Please view this email in an HTML-compatible viewer.";
                var plainView = AlternateView.CreateAlternateViewFromString(plainText, null, "text/plain");
                var htmlView = AlternateView.CreateAlternateViewFromString(htmlMessage, null, "text/html");

                mail.AlternateViews.Add(plainView);
                mail.AlternateViews.Add(htmlView);

                Console.WriteLine($"📧 Sending email to: {toEmail}");
                await smtpClient.SendMailAsync(mail);
                Console.WriteLine("✅ Email sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Failed to send email:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        public string GenerateResetPasswordEmailBody(string userName, string resetUrl)
        {
            return $@"
                <div style='font-family: Arial, sans-serif; color: #333;'>
                    <h2>Reset Your Password</h2>
                    <p>Hello <strong>{userName}</strong>,</p>
                    <p>You requested a password reset. Please click the button below:</p>
                    <p>
                        <a href='{resetUrl}' style='
                            background-color: #4CAF50;
                            color: white;
                            padding: 10px 20px;
                            text-decoration: none;
                            border-radius: 5px;
                            display: inline-block;'>Reset Password</a>
                    </p>
                    <p>If you did not request this, please ignore this email.</p>
                    <br/>
                    <p style='font-size: 12px; color: gray;'>– Tech Galaxy Team</p>
                </div>";
        }
    }
}
