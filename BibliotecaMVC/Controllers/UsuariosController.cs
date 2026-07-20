using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMVC.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
