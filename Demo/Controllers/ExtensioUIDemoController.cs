using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("extenso-ui-tests")]
    public class ExtensioUIDemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}