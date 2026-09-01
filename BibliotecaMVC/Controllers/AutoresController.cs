using BibliotecaMVC.Models;
using BibliotecaMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMVC.Controllers
{
    public class AutoresController : Controller
    {
        private readonly IAutorService _autorService;

        public AutoresController(IAutorService autorService)
        {
            _autorService = autorService;
        }

        public IActionResult Index()
        {
            return View(_autorService.ObtenerTodos());
        }

        public IActionResult Details(int id)
        {
            var autor = _autorService.ObtenerPorId(id);
            return autor is null ? NotFound() : View(autor);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Autor autor)
        {
            if (!ModelState.IsValid)
            {
                return View(autor);
            }

            _autorService.Crear(autor);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var autor = _autorService.ObtenerPorId(id);
            return autor is null ? NotFound() : View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Autor autor)
        {
            if (!ModelState.IsValid)
            {
                return View(autor);
            }

            return _autorService.Actualizar(autor)
                ? RedirectToAction(nameof(Index))
                : NotFound();
        }

        public IActionResult Delete(int id)
        {
            var autor = _autorService.ObtenerPorId(id);
            return autor is null ? NotFound() : View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _autorService.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
