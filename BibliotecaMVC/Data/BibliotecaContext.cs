using BibliotecaMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMVC.Data;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options)
    {
    }

    public DbSet<Libro> Libros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.Property(l => l.Precio).HasPrecision(10, 2);
        });
    }
}
