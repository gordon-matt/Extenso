using System.Diagnostics;
using Demo.Extenso.AspNetCore.Mvc.OData.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}