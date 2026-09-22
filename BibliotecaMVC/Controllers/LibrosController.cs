using BibliotecaMVC.Data;
using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMVC.Controllers
{
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var libros = _context.Libros
                .AsNoTracking()
                .OrderBy(libro => libro.Titulo)
                .ToList();
            return View(libros);
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

            if (string.IsNullOrWhiteSpace(libro.Imagen))
            {
                libro.Imagen = "odisea.jpg";
            }

            _context.Libros.Add(libro);
            _context.SaveChanges();
            TempData["MensajeExito"] = $"El libro \"{libro.Titulo}\" fue agregado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var libro = _context.Libros.Find(id);
            return libro is null ? NotFound() : View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Libro libro)
        {
            if (!ModelState.IsValid)
            {
                return View(libro);
            }

            var libroExistente = _context.Libros.Find(libro.Id);
            if (libroExistente is null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(libro.Imagen))
            {
                libro.Imagen = libroExistente.Imagen;
            }

            _context.Entry(libroExistente).CurrentValues.SetValues(libro);
            _context.Libros.Update(libroExistente);
            _context.SaveChanges();

            TempData["MensajeExito"] = $"El libro \"{libro.Titulo}\" fue actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var libro = _context.Libros.Find(id);
            return libro is null ? NotFound() : View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var libro = _context.Libros.Find(id);
            if (libro is null)
            {
                return NotFound();
            }

            _context.Libros.Remove(libro);
            _context.SaveChanges();

            TempData["MensajeExito"] = $"El libro \"{libro.Titulo}\" fue eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
