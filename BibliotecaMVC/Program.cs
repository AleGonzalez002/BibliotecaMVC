using BibliotecaMVC.Data;
using BibliotecaMVC.Repositories;
using BibliotecaMVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BibliotecaMVC.Security;
using BibliotecaMVC.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IRepositorioLibro, RepositorioEnMemoria>();
builder.Services.AddScoped<IAutorService, AutorServiceAlternativo>();
builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<BibliotecaContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Cuenta/Login";
    options.AccessDeniedPath = "/Cuenta/AccesoDenegado";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BibliotecaContext>();
    context.Database.EnsureCreated();

    // EnsureCreated no incorpora tablas nuevas cuando la base ya existía (por
    // ejemplo, una base que se creó antes de agregar el módulo Categorías).
    context.Database.ExecuteSqlRaw("""
        IF OBJECT_ID(N'dbo.Categorias', N'U') IS NULL
        BEGIN
            CREATE TABLE dbo.Categorias (
                Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Categorias PRIMARY KEY,
                Nombre nvarchar(100) NOT NULL,
                Descripcion nvarchar(250) NULL
            );
        END;
        """);

    if (!context.Libros.Any())
    {
        context.Libros.AddRange(
            new Libro { Titulo = "Don Quijote de la Mancha", Autor = "Miguel de Cervantes", Categoria = "Novela", Precio = 19.99m, Disponible = true, Imagen = "donquijote.jpg" },
            new Libro { Titulo = "La Odisea", Autor = "Homero", Categoria = "Epopeya", Precio = 14.99m, Disponible = true, Imagen = "odisea.jpg" },
            new Libro { Titulo = "Orgullo y prejuicio", Autor = "Jane Austen", Categoria = "Romance", Precio = 12.99m, Disponible = true, Imagen = "orgullo.jpg" });
        context.SaveChanges();
    }

    context.Database.ExecuteSqlRaw("""
        IF NOT EXISTS (SELECT 1 FROM dbo.Categorias)
        BEGIN
            INSERT INTO dbo.Categorias (Nombre, Descripcion) VALUES
                (N'Novela', N'Obras narrativas de ficción.'),
                (N'Epopeya', N'Relatos extensos de tradición épica.'),
                (N'Romance', N'Historias centradas en relaciones y sentimientos.');
        END;
        """);

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    foreach (var role in Roles.Todos)
    {
        if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
        {
            roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
        }
    }

    // Usuarios existentes y futuros quedan como lectores; el correo configurado
    // en Roles:AdministradorEmail recibe el rol de administración.
    foreach (var user in userManager.Users.ToList())
    {
        if (!userManager.GetRolesAsync(user).GetAwaiter().GetResult().Any())
        {
            userManager.AddToRoleAsync(user, Roles.Usuario).GetAwaiter().GetResult();
        }
    }

    foreach (var (clave, rol) in new[]
    {
        ("Roles:AdministradorEmail", Roles.Administrador),
        ("Roles:BibliotecarioEmail", Roles.Bibliotecario)
    })
    {
        var correo = app.Configuration[clave];
        if (!string.IsNullOrWhiteSpace(correo))
        {
            var usuario = userManager.FindByEmailAsync(correo).GetAwaiter().GetResult();
            if (usuario is not null && !userManager.IsInRoleAsync(usuario, rol).GetAwaiter().GetResult())
            {
                userManager.AddToRoleAsync(usuario, rol).GetAwaiter().GetResult();
            }
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
