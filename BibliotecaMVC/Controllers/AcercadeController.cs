using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMVC.Controllers
{
    public class AcercadeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
