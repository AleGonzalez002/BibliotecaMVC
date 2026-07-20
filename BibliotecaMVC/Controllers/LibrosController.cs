using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    public class LibrosController : Controller
    {
        public IActionResult Index()
        {
            List<Libro> libros = new List<Libro>()
            {
                new Libro { Id = 1,
                Titulo = "Odisea",
                Autor = "Homero",
                Categoria = "Epopeya",
                Precio = 35.6M,
                Disponible = true
            },
                 new Libro { Id = 2,
                Titulo = "Odisea 2",
                Autor = "Homero 2",
                Categoria = "Epopeya 2",
                Precio = 35.6M,
                Disponible = false
            }
            };
            ViewBag.Libros = libros;
            return View();
        }
    }
}
