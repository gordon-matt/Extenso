using System.ComponentModel.DataAnnotations;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}