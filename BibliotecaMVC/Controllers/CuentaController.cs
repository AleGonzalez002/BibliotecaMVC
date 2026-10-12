using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BibliotecaMVC.Security;

namespace BibliotecaMVC.Controllers
{
    public class CuentaController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public CuentaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = await userManager.FindByNameAsync(modelo.UsuarioOCorreo);
            if (usuario == null)
            {
                usuario = await userManager.FindByEmailAsync(modelo.UsuarioOCorreo);
            }

            if (usuario != null)
            {
                var resultado = await signInManager.PasswordSignInAsync(usuario.UserName!, modelo.Password, false, false);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Usuario, correo o contraseña incorrectos.");
            return View(modelo);
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = new IdentityUser
            {
                UserName = modelo.Usuario,
                Email = modelo.Correo
            };

            var resultado = await userManager.CreateAsync(usuario, modelo.Password);
            if (resultado.Succeeded)
            {
                await userManager.AddToRoleAsync(usuario, Roles.Usuario);
                await signInManager.SignInAsync(usuario, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }
}
