using CasoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CasoMVC.Controllers
{
    public class EventosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventosController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool UsuarioEsAdministrador()
        {
            return HttpContext.Session.GetString("Rol") == "Admin";
        }

        // GET: Eventos
        public async Task<IActionResult> Index()
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            var eventos = await _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.UsuarioRegistro)
                .ToListAsync();

            return View(eventos);
        }

        // GET: Eventos/Create
        public IActionResult Create()
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            ViewData["Categorias"] = _context.Categorias.ToList();
            return View();
        }

        // POST: Eventos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evento evento)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            if (evento.Fecha < DateTime.Today)
                ModelState.AddModelError("Fecha", "La fecha no puede estar en el pasado.");

            if (!TimeSpan.TryParse(evento.Duracion, out TimeSpan duracion) || duracion.TotalMinutes <= 0)
                ModelState.AddModelError("Duracion", "La duración debe ser válida y positiva.");

            if (evento.CupoMaximo <= 0)
                ModelState.AddModelError("CupoMaximo", "El cupo máximo debe ser mayor a 0.");

            if (ModelState.IsValid)
            {
                evento.FechaRegistro = DateTime.Now;

                if (int.TryParse(HttpContext.Session.GetString("UsuarioId"), out int usuarioId))
                {
                    evento.UsuarioRegistroId = usuarioId;
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo obtener el usuario desde la sesión.");
                    ViewData["Categorias"] = _context.Categorias.ToList();
                    return View(evento);
                }

                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Categorias"] = _context.Categorias.ToList();
            return View(evento);
        }

        // GET: Eventos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            if (id == null)
                return NotFound();

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
                return NotFound();

            ViewData["Categorias"] = _context.Categorias.ToList();
            return View(evento);
        }

        // POST: Eventos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Evento evento)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            if (id != evento.Id)
                return NotFound();

            if (evento.Fecha < DateTime.Today)
                ModelState.AddModelError("Fecha", "La fecha no puede estar en el pasado.");

            if (!TimeSpan.TryParse(evento.Duracion, out TimeSpan duracion) || duracion.TotalMinutes <= 0)
                ModelState.AddModelError("Duracion", "La duración debe ser válida y positiva.");

            if (evento.CupoMaximo <= 0)
                ModelState.AddModelError("CupoMaximo", "El cupo máximo debe ser mayor a 0.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Eventos.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
            }

            ViewData["Categorias"] = _context.Categorias.ToList();
            return View(evento);
        }

        // GET: Eventos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            if (id == null)
                return NotFound();

            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.UsuarioRegistro)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (evento == null)
                return NotFound();

            return View(evento);
        }

        // GET: Eventos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            if (id == null)
                return NotFound();

            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.UsuarioRegistro)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (evento == null)
                return NotFound();

            return View(evento);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToAction("Login", "Auth");

            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}