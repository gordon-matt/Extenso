using System.Diagnostics;
using Demo.Extenso.AspNetCore.Mvc.OData.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}