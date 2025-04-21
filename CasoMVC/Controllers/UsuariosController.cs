using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CasoModels;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using CasoMVC.Models;

namespace CasoMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool UsuarioEsAdministrador()
        {
            return HttpContext.Session.GetString("Rol") == "Admin";
        }

        public async Task<IActionResult> Index()
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            var usuarios = await _context.Usuarios
                .Select(u => new Usuario
                {
                    Id = u.Id,
                    NombreUsuario = u.NombreUsuario,
                    NombreCompleto = u.NombreCompleto,
                    Correo = u.Correo,
                    Telefono = u.Telefono,
                    Rol = u.Rol
                }).ToListAsync();

            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Console.WriteLine("🟢 [GET] Accediendo a la vista Crear Usuario");

            if (!UsuarioEsAdministrador())
            {
                Console.WriteLine("🔒 Usuario no autorizado. Redirigiendo a Login.");
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel model)
        {
            Console.WriteLine("📩 [POST] Recibiendo datos del formulario para crear usuario");

            if (!UsuarioEsAdministrador())
            {
                Console.WriteLine("🔒 Usuario no autorizado al intentar crear usuario. Redirigiendo.");
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                var usuario = new Usuario
                {
                    NombreUsuario = model.NombreUsuario,
                    NombreCompleto = model.NombreCompleto,
                    Correo = model.Correo,
                    Telefono = model.Telefono,
                    Contrasena = model.Contrasena,
                    Rol = model.Rol // Ahora el rol viene del formulario
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                Console.WriteLine("💾 Usuario creado exitosamente.");
                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine("❌ Modelo inválido. Detalles:");
            foreach (var error in ModelState.Values.SelectMany(e => e.Errors))
            {
                Console.WriteLine($"   - {error.ErrorMessage}");
            }

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            var model = new UsuarioViewModel
            {
                NombreUsuario = usuario.NombreUsuario,
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Contrasena = usuario.Contrasena,
                Rol = usuario.Rol
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioViewModel model)
        {
            if (!UsuarioEsAdministrador())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                usuario.NombreUsuario = model.NombreUsuario;
                usuario.NombreCompleto = model.NombreCompleto;
                usuario.Correo = model.Correo;
                usuario.Telefono = model.Telefono;
                usuario.Contrasena = model.Contrasena;
                usuario.Rol = model.Rol;

                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Usuarios.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null) return NotFound();

            var model = new UsuarioViewModel
            {
                NombreUsuario = usuario.NombreUsuario,
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Contrasena = usuario.Contrasena,
                Rol = usuario.Rol
            };

            ViewBag.UsuarioId = usuario.Id; 

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null) return NotFound();

            var model = new UsuarioViewModel
            {
                NombreUsuario = usuario.NombreUsuario,
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Contrasena = usuario.Contrasena,
                Rol = usuario.Rol
            };

            return View(model);
        }

    }
}
