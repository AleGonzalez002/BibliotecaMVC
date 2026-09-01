using BibliotecaMVC.Models;

namespace BibliotecaMVC.Repositories
{
    public class RepositorioEnMemoria : IRepositorioLibro
    {
        public IEnumerable<Libro> ObtenerTodos()
        {
            return new List<Libro>
            {
                new Libro { Id = 1, Titulo = "Cien Años de Soledad", Autor = "Gabriel García Márquez", Categoria = "Novela", Precio = 19.99m, Disponible = true, Imagen = "cien_anos_de_soledad.jpg" },
                new Libro { Id = 2, Titulo = "1984", Autor = "George Orwell", Categoria = "Distopía", Precio = 14.99m, Disponible = true, Imagen = "1984.jpg" },
                new Libro { Id = 3, Titulo = "El Principito", Autor = "Antoine de Saint-Exupéry", Categoria = "Fantasía", Precio = 9.99m, Disponible = false, Imagen = "el_principito.jpg" }
            };
        }
    }
}
