using System;
using System.Threading.Tasks;

namespace W_M_S_Project.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // MOCK Email Service
            Console.WriteLine($"--- Email Mock Simulation ---");
            Console.WriteLine($"To: {toEmail}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body: {body}");
            Console.WriteLine($"-----------------------------");

            return Task.CompletedTask;
        }
    }
}
