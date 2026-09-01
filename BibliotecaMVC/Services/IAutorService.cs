using BibliotecaMVC.Models;

namespace BibliotecaMVC.Services
{
    public interface IAutorService
    {
        IEnumerable<Autor> ObtenerTodos();
        Autor? ObtenerPorId(int id);
        void Crear(Autor autor);
        bool Actualizar(Autor autor);
        void Eliminar(int id);
    }
}
