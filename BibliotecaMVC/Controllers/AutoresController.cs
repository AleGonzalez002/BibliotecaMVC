using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    public class AutoresController : Controller
    {
        public IActionResult Index()
        {
            List<Autor> autores = new List<Autor>()
            {
                new Autor { 
                Id = 1,
                Nombre = "Dennis",
                Apellido = "Gonzalez",
                Nacionalidad = "El salvador",
                FechaNacimiento= new DateTime(2006, 9, 21),
                Activo = true
            },
               new Autor
            {
                Id = 2,
                Nombre = "William",
                Apellido = "Shakespeare",
                Nacionalidad = "Reino Unido",
                FechaNacimiento = new DateTime(1564, 4, 26),
                Activo = true
            },
            new Autor
            {
                Id = 3,
                Nombre = "Miguel",
                Apellido = "de Cervantes",
                Nacionalidad = "España",
                FechaNacimiento = new DateTime(1547, 9, 29),
                Activo = true
            },
            new Autor
            {
                Id = 4,
                Nombre = "Fiódor",
                Apellido = "Dostoyevski",
                Nacionalidad = "Rusia",
                FechaNacimiento = new DateTime(1821, 11, 11),
                Activo = false
            },
            new Autor
            {
                Id = 5,
                Nombre = "Jane",
                Apellido = "Austen",
                Nacionalidad = "Reino Unido",
                FechaNacimiento = new DateTime(1775, 12, 16),
                Activo = false
            },

            };
            ViewBag.autores = autores;
            return View();
        }
    }
}
