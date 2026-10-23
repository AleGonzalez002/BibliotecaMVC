using System.Diagnostics;
using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace BibliotecaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

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
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exception is not null)
            {
                _logger.LogError(exception.Error, "Error no controlado al procesar la ruta {Ruta}.", exception.Path);
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
