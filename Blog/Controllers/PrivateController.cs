using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class PrivateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
