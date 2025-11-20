using System.Text.Encodings.Web;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Services;

public static class EmailSenderExtensions
{
    extension(IEmailSender emailSender)
    {
        public Task SendEmailConfirmationAsync(string email, string link) =>
            emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
    }
}