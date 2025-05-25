using System.Diagnostics;
using Demo.AspNetCore.Mvc.ExtensoUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.AspNetCore.Mvc.ExtensoUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index() => View();

    public IActionResult Bootstrap3() => View();

    public IActionResult Bootstrap4() => View();

    public IActionResult Bootstrap5() => View();

    public IActionResult Foundation6() => View();

    public IActionResult JQueryUI() => View();

    public IActionResult KendoBootstrap3() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}