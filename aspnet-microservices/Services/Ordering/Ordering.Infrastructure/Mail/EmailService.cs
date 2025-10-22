using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public async Task<bool> SendEmailAsync(Email email)
        {
            return true;
        }
    }
}
