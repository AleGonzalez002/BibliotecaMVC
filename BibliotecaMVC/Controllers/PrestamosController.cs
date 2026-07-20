using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMVC.Controllers
{
    public class PrestamosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
