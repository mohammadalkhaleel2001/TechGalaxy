﻿namespace TechGalaxyProject.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);

    }
}
