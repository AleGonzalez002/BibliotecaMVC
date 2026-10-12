namespace BibliotecaMVC.Security;

public static class Roles
{
    public const string Administrador = "Administrador";
    public const string Bibliotecario = "Bibliotecario";
    public const string Usuario = "Usuario";

    public static readonly string[] Todos = [Administrador, Bibliotecario, Usuario];
    public const string GestionLibros = Administrador + "," + Bibliotecario;
}
