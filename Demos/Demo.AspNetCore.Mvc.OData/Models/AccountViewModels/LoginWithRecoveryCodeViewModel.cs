using System.ComponentModel.DataAnnotations;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Models.AccountViewModels;

public class LoginWithRecoveryCodeViewModel
{
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Recovery Code")]
    public string RecoveryCode { get; set; }
}