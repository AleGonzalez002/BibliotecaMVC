using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMVC.Controllers
{
    public class LibrosController : Controller
    {
        private static readonly List<Libro> libros = new List<Libro>()
        {
            new Libro { Id = 1, Titulo = "La Odisea", Autor = "Homero", Categoria = "Epopeya", Precio = 35.60M, Disponible = true, Imagen = "odisea.jpg" },
            new Libro { Id = 2, Titulo = "Don Quijote de la Mancha", Autor = "Miguel de Cervantes", Categoria = "Novela", Precio = 28.50M, Disponible = true, Imagen = "donquijote.jpg" },
            new Libro { Id = 3, Titulo = "Orgullo y prejuicio", Autor = "Jane Austen", Categoria = "Romance", Precio = 22.00M, Disponible = false, Imagen = "orgullo.jpg" }
        };

        public IActionResult Index()
        {
            return View(libros);
        }

        public IActionResult Details(int id)
        {
            var libro = libros.FirstOrDefault(x => x.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Libro libro)
        {
            if (!ModelState.IsValid)
            {
                return View(libro);
            }

            libro.Id = libros.Any() ? libros.Max(x => x.Id) + 1 : 1;
            if (string.IsNullOrWhiteSpace(libro.Imagen))
            {
                libro.Imagen = "odisea.jpg";
            }

            libros.Add(libro);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var libro = libros.FirstOrDefault(x => x.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Libro libroModificado)
        {
            if (!ModelState.IsValid)
            {
                return View(libroModificado);
            }

            var libroExistente = libros.FirstOrDefault(x => x.Id == libroModificado.Id);
            if (libroExistente == null)
            {
                return NotFound();
            }

            libroExistente.Titulo = libroModificado.Titulo;
            libroExistente.Autor = libroModificado.Autor;
            libroExistente.Categoria = libroModificado.Categoria;
            libroExistente.Precio = libroModificado.Precio;
            libroExistente.Disponible = libroModificado.Disponible;
            libroExistente.Imagen = libroModificado.Imagen;

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var libro = libros.FirstOrDefault(x => x.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var libro = libros.FirstOrDefault(x => x.Id == id);
            if (libro != null)
            {
                libros.Remove(libro);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
