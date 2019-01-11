using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BLL.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, IConfiguration configuration);
    }
}