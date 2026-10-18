namespace BibliotecaMVC.Security;

public static class Roles
{
    // El nombre del rol sigue la matriz de permisos de la actividad.
    public const string Administrador = "Admin";
    public const string Bibliotecario = "Bibliotecario";
    public const string Usuario = "Usuario";

    public static readonly string[] Todos = [Administrador, Bibliotecario, Usuario];
    public const string GestionLibros = Administrador + "," + Bibliotecario;
}
