using Microsoft.AspNetCore.Mvc;

namespace Demo.AspNetCore.Mvc.ExtensoUI.Bootstrap3.Controllers
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