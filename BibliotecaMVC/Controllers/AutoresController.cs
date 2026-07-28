using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    public class AutoresController : Controller
    {
        private static readonly List<Autor> autores = new List<Autor>()
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
            }
        };

        public IActionResult Index()
        {
            return View(autores);
        }

        public IActionResult Details(int id)
        {
            var autor = autores.FirstOrDefault(x => x.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Autor autor)
        {
            if (!ModelState.IsValid)
            {
                return View(autor);
            }

            if (autores.Any())
            {
                autor.Id = autores.Max(x => x.Id) + 1;
            }
            else
            {
                autor.Id = 1;
            }

            autores.Add(autor);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var autor = autores.FirstOrDefault(x => x.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Autor autorModificado)
        {
            if (!ModelState.IsValid)
            {
                return View(autorModificado);
            }

            var autorExistente = autores.FirstOrDefault(x => x.Id == autorModificado.Id);
            if (autorExistente == null)
            {
                return NotFound();
            }

            autorExistente.Nombre = autorModificado.Nombre;
            autorExistente.Apellido = autorModificado.Apellido;
            autorExistente.Nacionalidad = autorModificado.Nacionalidad;
            autorExistente.FechaNacimiento = autorModificado.FechaNacimiento;
            autorExistente.Activo = autorModificado.Activo;

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var autor = autores.FirstOrDefault(x => x.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var autor = autores.FirstOrDefault(x => x.Id == id);
            if (autor != null)
            {
                autores.Remove(autor); 
            }

            return RedirectToAction(nameof(Index));
        }
    }
}