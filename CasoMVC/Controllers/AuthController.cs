using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CasoModels;
using CasoMVC.Models;
using System.Threading.Tasks;
using System.Linq;

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
        [HttpGet]
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
                    .FirstOrDefaultAsync(u =>
                        u.NombreUsuario == model.NombreUsuario &&
                        u.Contrasena == model.Contrasena);

                if (usuario != null)
                {
                    HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
                    HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);
                    HttpContext.Session.SetString("Rol", usuario.Rol);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Credenciales inválidas");
            }

            return View(model);
        }

        // GET: Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Validar nombre de usuario duplicado
                if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == model.NombreUsuario))
                {
                    ModelState.AddModelError("NombreUsuario", "Este nombre de usuario ya está registrado.");
                    return View(model);
                }

                // Validar correo duplicado
                if (await _context.Usuarios.AnyAsync(u => u.Correo == model.Correo))
                {
                    ModelState.AddModelError("Correo", "Este correo ya está registrado.");
                    return View(model);
                }

                // Crear el usuario real desde el modelo de vista
                var usuario = new Usuario
                {
                    NombreUsuario = model.NombreUsuario,
                    NombreCompleto = model.NombreCompleto,
                    Correo = model.Correo,
                    Telefono = model.Telefono,
                    Contrasena = model.Contrasena,
                    Rol = "Usuario" // Asignación automática
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }
    }
}
