using Microsoft.AspNetCore.Mvc;
using CasoModels;
using CasoMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CasoMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.NombreUsuario == model.NombreUsuario && u.Contrasena == model.Contrasena);

                if (usuario != null)
                {
                    HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
                    HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);
                    HttpContext.Session.SetString("Rol", usuario.Rol);

                    // Redirección según rol
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Credenciales inválidas");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
