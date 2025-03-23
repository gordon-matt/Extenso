namespace Demo.AspNetCore.Mvc.ExtensoUI.Bootstrap5.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}