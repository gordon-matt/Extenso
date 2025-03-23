using Microsoft.AspNetCore.Mvc;

namespace Demo.AspNetCore.Mvc.ExtensoUI.KendoBootstrap3.Controllers;

public class HomeController : Controller
{
    [Route("")]
    public IActionResult Index() => View();
}