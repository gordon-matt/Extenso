using Microsoft.AspNetCore.Mvc;

namespace Demo.AspNetCore.Mvc.ExtensoUI.JQueryUI.Controllers;

public class HomeController : Controller
{
    [Route("")]
    public IActionResult Index() => View();
}