using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BibliotecaMVC.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly string _cadenaConexion;

        public CategoriasController(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión DefaultConnection.");
        }

        public IActionResult Index()
        {
            var categorias = new List<Categoria>();
            const string consulta = "SELECT Id, Nombre, Descripcion FROM Categorias ORDER BY Nombre";

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
            return View(categorias);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (!ModelState.IsValid) return View(categoria);

            const string consulta = "INSERT INTO Categorias (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";
            using var conexion = new SqlConnection(_cadenaConexion);
            using var comando = new SqlCommand(consulta, conexion);
            comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = categoria.Nombre;
            comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 250).Value = (object?)categoria.Descripcion ?? DBNull.Value;
            conexion.Open();
            comando.ExecuteNonQuery();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var categoria = ObtenerPorId(id);
            return categoria is null ? NotFound() : View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (!ModelState.IsValid) return View(categoria);

            const string consulta = "UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Descripcion WHERE Id = @Id";
            using var conexion = new SqlConnection(_cadenaConexion);
            using var comando = new SqlCommand(consulta, conexion);
            comando.Parameters.Add("@Id", SqlDbType.Int).Value = categoria.Id;
            comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = categoria.Nombre;
            comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 250).Value = (object?)categoria.Descripcion ?? DBNull.Value;
            conexion.Open();
            return comando.ExecuteNonQuery() == 1 ? RedirectToAction(nameof(Index)) : NotFound();
        }

        public IActionResult Delete(int id)
        {
            var categoria = ObtenerPorId(id);
            return categoria is null ? NotFound() : View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            const string consulta = "DELETE FROM Categorias WHERE Id = @Id";
            using var conexion = new SqlConnection(_cadenaConexion);
            using var comando = new SqlCommand(consulta, conexion);
            comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            conexion.Open();
            return comando.ExecuteNonQuery() == 1 ? RedirectToAction(nameof(Index)) : NotFound();
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
