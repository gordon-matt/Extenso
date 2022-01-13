using System.Threading.Tasks;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}