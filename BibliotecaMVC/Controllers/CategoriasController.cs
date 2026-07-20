using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMVC.Controllers
{
    public class CategoriasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
