using System.ComponentModel.DataAnnotations;

namespace BibliotecaMVC.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Autor { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Categoria { get; set; } = string.Empty;

        [Range(0.01, 1000)]
        public decimal Precio { get; set; }

        public bool Disponible { get; set; }

        [StringLength(150)]
        public string Imagen { get; set; } = string.Empty;
    }
}
