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
            return RedirectToAction(nameof(Index));
        }
    }
}
