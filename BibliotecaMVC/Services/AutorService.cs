using BibliotecaMVC.Models;

namespace BibliotecaMVC.Services
{
    public class AutorService : IAutorService
    {
        // Datos temporales en memoria para el ejemplo. Más adelante pueden sustituirse por una base de datos.
        private static readonly List<Autor> autores = new()
        {
            new Autor { Id = 1, Nombre = "Dennis", Apellido = "Gonzalez", Nacionalidad = "El Salvador", FechaNacimiento = new DateTime(2006, 9, 21), Activo = true },
            new Autor { Id = 2, Nombre = "William", Apellido = "Shakespeare", Nacionalidad = "Reino Unido", FechaNacimiento = new DateTime(1564, 4, 26), Activo = true },
            new Autor { Id = 3, Nombre = "Miguel", Apellido = "de Cervantes", Nacionalidad = "España", FechaNacimiento = new DateTime(1547, 9, 29), Activo = true },
            new Autor { Id = 4, Nombre = "Fiódor", Apellido = "Dostoyevski", Nacionalidad = "Rusia", FechaNacimiento = new DateTime(1821, 11, 11), Activo = false },
            new Autor { Id = 5, Nombre = "Jane", Apellido = "Austen", Nacionalidad = "Reino Unido", FechaNacimiento = new DateTime(1775, 12, 16), Activo = false }
        };

        public virtual IEnumerable<Autor> ObtenerTodos() => autores;

        public Autor? ObtenerPorId(int id) => autores.FirstOrDefault(a => a.Id == id);

        public void Crear(Autor autor)
        {
            autor.Id = autores.Any() ? autores.Max(a => a.Id) + 1 : 1;
            autores.Add(autor);
        }

        public bool Actualizar(Autor autor)
        {
            var autorExistente = ObtenerPorId(autor.Id);
            if (autorExistente is null)
            {
                return false;
            }

            autorExistente.Nombre = autor.Nombre;
            autorExistente.Apellido = autor.Apellido;
            autorExistente.Nacionalidad = autor.Nacionalidad;
            autorExistente.FechaNacimiento = autor.FechaNacimiento;
            autorExistente.Activo = autor.Activo;
            return true;
        }

        public void Eliminar(int id)
        {
            var autor = ObtenerPorId(id);
            if (autor is not null)
            {
                autores.Remove(autor);
            }
        }
    }
}
