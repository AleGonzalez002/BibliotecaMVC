using BibliotecaMVC.Data;
using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BibliotecaMVC.Security;

namespace BibliotecaMVC.Controllers
{
    [Authorize(Roles = Roles.Administrador + "," + Roles.Bibliotecario + "," + Roles.Usuario)]
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;
        private readonly ILogger<LibrosController> _logger;

        public LibrosController(BibliotecaContext context, ILogger<LibrosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Consultando el catálogo de libros.");
            var libros = _context.Libros
                .AsNoTracking()
                .OrderBy(libro => libro.Titulo)
                .ToList();
            return View(libros);
        }

        public IActionResult Details(int id)
        {
            _logger.LogInformation("Consultando los detalles del libro {LibroId}.", id);
            var libro = _context.Libros.AsNoTracking().FirstOrDefault(item => item.Id == id);
            if (libro is null) _logger.LogWarning("No se encontraron detalles para el libro {LibroId}.", id);
            return libro is null ? NotFound() : View(libro);
        }

        [Authorize(Roles = Roles.GestionLibros)]
        public IActionResult Create()
        {
            _logger.LogInformation("Mostrando el formulario para crear un libro.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.GestionLibros)]
        public IActionResult Create(Libro libro)
        {
            _logger.LogInformation("Procesando la creación de un libro.");
            if (!ModelState.IsValid)
            {
                return View(libro);
            }

            if (string.IsNullOrWhiteSpace(libro.Imagen))
            {
                libro.Imagen = "odisea.jpg";
            }

            try
            {
                _context.Libros.Add(libro);
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ocurrió un error al crear un libro.");
                throw;
            }
            _logger.LogInformation("Se creó el libro {LibroId}.", libro.Id);
            TempData["MensajeExito"] = $"El libro \"{libro.Titulo}\" fue agregado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = Roles.GestionLibros)]
        public IActionResult Edit(int id)
        {
            _logger.LogInformation("Consultando el libro {LibroId} para editarlo.", id);
            var libro = _context.Libros.Find(id);
            if (libro is null) _logger.LogWarning("No se encontró el libro {LibroId} para editarlo.", id);
            return libro is null ? NotFound() : View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.GestionLibros)]
        public IActionResult Edit(Libro libro)
        {
            _logger.LogInformation("Procesando la edición del libro {LibroId}.", libro.Id);
            if (!ModelState.IsValid)
            {
                return View(libro);
            }

            var libroExistente = _context.Libros.Find(libro.Id);
            if (libroExistente is null)
            {
                _logger.LogWarning("No se encontró el libro {LibroId} solicitado para edición.", libro.Id);
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(libro.Imagen))
            {
                libro.Imagen = libroExistente.Imagen;
            }

            try
            {
                _context.Entry(libroExistente).CurrentValues.SetValues(libro);
                _context.Libros.Update(libroExistente);
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ocurrió un error al editar el libro {LibroId}.", libro.Id);
                throw;
            }
            _logger.LogInformation("Se actualizó el libro {LibroId}.", libro.Id);

            TempData["MensajeExito"] = $"El libro \"{libro.Titulo}\" fue actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = Roles.GestionLibros)]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Consultando el libro {LibroId} para eliminarlo.", id);
            var libro = _context.Libros.Find(id);
            if (libro is null) _logger.LogWarning("No se encontró el libro {LibroId} para eliminarlo.", id);
            return libro is null ? NotFound() : View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.GestionLibros)]
        public IActionResult DeleteConfirmed(int id)
        {
            _logger.LogInformation("Procesando la eliminación del libro {LibroId}.", id);
            var libro = _context.Libros.Find(id);
            if (libro is null)
            {
                _logger.LogWarning("No se encontró el libro {LibroId} solicitado para eliminación.", id);
                return NotFound();
            }

            try
            {
                _context.Libros.Remove(libro);
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ocurrió un error al eliminar el libro {LibroId}.", id);
                throw;
            }
            _logger.LogInformation("Se eliminó el libro {LibroId}.", id);

            TempData["MensajeExito"] = $"El libro \"{libro.Titulo}\" fue eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
