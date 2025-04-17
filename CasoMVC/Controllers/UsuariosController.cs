using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CasoModels;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

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
            return HttpContext.Session.GetString("Rol") == "Administrador";
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

        public IActionResult Create()
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreUsuario,NombreCompleto,Correo,Telefono,Contrasena,Rol")] Usuario usuario)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreUsuario,NombreCompleto,Correo,Telefono,Contrasena,Rol")] Usuario usuario)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            if (id != usuario.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Usuarios.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            if (id == null) return NotFound();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            if (id == null) return NotFound();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }
    }
}
