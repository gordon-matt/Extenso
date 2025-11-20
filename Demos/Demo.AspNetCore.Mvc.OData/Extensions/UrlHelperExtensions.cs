using Demo.Extenso.AspNetCore.Mvc.OData.Controllers;

namespace Microsoft.AspNetCore.Mvc;

public static class UrlHelperExtensions
{
    extension(IUrlHelper urlHelper)
    {
        public string EmailConfirmationLink(string userId, string code, string scheme) => urlHelper.Action(
            action: nameof(AccountController.ConfirmEmail),
            controller: "Account",
            values: new { userId, code },
            protocol: scheme);

        public string ResetPasswordCallbackLink(string userId, string code, string scheme) => urlHelper.Action(
            action: nameof(AccountController.ResetPassword),
            controller: "Account",
            values: new { userId, code },
            protocol: scheme);
    }
}