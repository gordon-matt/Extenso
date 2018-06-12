using Microsoft.AspNetCore.Mvc;

namespace Demo.AspNetCore.Mvc.ExtensoUI.Bootstrap4.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}