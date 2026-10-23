using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BibliotecaMVC.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly string _cadenaConexion;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _logger = logger;
            _cadenaConexion = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión DefaultConnection.");
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Consultando el listado de categorías.");
            var categorias = new List<Categoria>();
            const string consulta = "SELECT Id, Nombre, Descripcion FROM Categorias ORDER BY Nombre";

            try
            {
                using var conexion = new SqlConnection(_cadenaConexion);
                using var comando = new SqlCommand(consulta, conexion);
                conexion.Open();
                using var lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    categorias.Add(new Categoria
                    {
                        Id = lector.GetInt32(0),
                        Nombre = lector.GetString(1),
                        Descripcion = lector.IsDBNull(2) ? string.Empty : lector.GetString(2)
                    });
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ocurrió un error al consultar las categorías.");
                throw;
            }
            return View(categorias);
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Mostrando el formulario para crear una categoría.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            _logger.LogInformation("Procesando la creación de una categoría.");
            if (!ModelState.IsValid) return View(categoria);

            const string consulta = "INSERT INTO Categorias (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";
            try
            {
                using var conexion = new SqlConnection(_cadenaConexion);
                using var comando = new SqlCommand(consulta, conexion);
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = categoria.Nombre;
                comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 250).Value = (object?)categoria.Descripcion ?? DBNull.Value;
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ocurrió un error al crear una categoría.");
                throw;
            }
            _logger.LogInformation("Se creó una categoría.");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            _logger.LogInformation("Consultando la categoría {CategoriaId} para editarla.", id);
            var categoria = ObtenerPorId(id);
            if (categoria is null) _logger.LogWarning("No se encontró la categoría {CategoriaId} para editarla.", id);
            return categoria is null ? NotFound() : View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            _logger.LogInformation("Procesando la edición de la categoría {CategoriaId}.", categoria.Id);
            if (!ModelState.IsValid) return View(categoria);

            const string consulta = "UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Descripcion WHERE Id = @Id";
            try
            {
                using var conexion = new SqlConnection(_cadenaConexion);
                using var comando = new SqlCommand(consulta, conexion);
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = categoria.Id;
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = categoria.Nombre;
                comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 250).Value = (object?)categoria.Descripcion ?? DBNull.Value;
                conexion.Open();
                if (comando.ExecuteNonQuery() == 1)
                {
                    _logger.LogInformation("Se actualizó la categoría {CategoriaId}.", categoria.Id);
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogWarning("No se encontró la categoría {CategoriaId} solicitada para edición.", categoria.Id);
                return NotFound();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ocurrió un error al editar la categoría {CategoriaId}.", categoria.Id);
                throw;
            }
        }

        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Consultando la categoría {CategoriaId} para eliminarla.", id);
            var categoria = ObtenerPorId(id);
            if (categoria is null) _logger.LogWarning("No se encontró la categoría {CategoriaId} para eliminarla.", id);
            return categoria is null ? NotFound() : View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _logger.LogInformation("Procesando la eliminación de la categoría {CategoriaId}.", id);
            const string consulta = "DELETE FROM Categorias WHERE Id = @Id";
            try
            {
                using var conexion = new SqlConnection(_cadenaConexion);
                using var comando = new SqlCommand(consulta, conexion);
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                conexion.Open();
                if (comando.ExecuteNonQuery() == 1)
                {
                    _logger.LogInformation("Se eliminó la categoría {CategoriaId}.", id);
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogWarning("No se encontró la categoría {CategoriaId} solicitada para eliminación.", id);
                return NotFound();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ocurrió un error al eliminar la categoría {CategoriaId}.", id);
                throw;
            }
        }

        private Categoria? ObtenerPorId(int id)
        {
            const string consulta = "SELECT Id, Nombre, Descripcion FROM Categorias WHERE Id = @Id";
            using var conexion = new SqlConnection(_cadenaConexion);
            using var comando = new SqlCommand(consulta, conexion);
            comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            conexion.Open();
            using var lector = comando.ExecuteReader();

            return lector.Read()
                ? new Categoria
                {
                    Id = lector.GetInt32(0),
                    Nombre = lector.GetString(1),
                    Descripcion = lector.IsDBNull(2) ? string.Empty : lector.GetString(2)
                }
                : null;
        }
    }
}
