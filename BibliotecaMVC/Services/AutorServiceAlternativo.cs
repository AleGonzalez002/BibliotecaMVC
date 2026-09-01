using BibliotecaMVC.Models;

namespace BibliotecaMVC.Services
{
    // Segunda implementación: cambia el orden del listado sin cambiar el controlador.
    public class AutorServiceAlternativo : AutorService
    {
        public override IEnumerable<Autor> ObtenerTodos() => base.ObtenerTodos().OrderBy(a => a.Nombre);
    }
}
