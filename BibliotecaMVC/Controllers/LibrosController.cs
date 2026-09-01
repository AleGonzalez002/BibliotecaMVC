using BibliotecaMVC.Models;
using BibliotecaMVC.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMVC.Controllers
{
    public class LibrosController : Controller
    {
        private readonly IRepositorioLibro _repositorioLibro;

        public LibrosController(IRepositorioLibro repositorioLibro)
        {
            _repositorioLibro = repositorioLibro;
        }

        public IActionResult Index()
        {
            var libros = _repositorioLibro.ObtenerTodos();
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
