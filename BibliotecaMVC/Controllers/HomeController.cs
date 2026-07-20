using System.Diagnostics;
using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMVC.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Autores()
        {
            return View();
        }

        public IActionResult Categorias()
        {
            return View();

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Usuarios()
        {
            return View();

        }

        public IActionResult Prestamos()
        {
            return View();
        }

        public IActionResult Acercade()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
