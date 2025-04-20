using CasoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CasoMVC.Controllers
{
    public class InscripcionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InscripcionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int ObtenerUsuarioIdActual()
        {
            var userID = HttpContext.Session.GetString("UsuarioId");
            if (int.TryParse(userID, out int usuarioId))
            {
                return usuarioId;
            }
            throw new Exception("No se pudo obtener el ID del usuario actual desde la sesión.");
        }

        private bool UsuarioEsAdministrador()
        {
            return HttpContext.Session.GetString("Rol") == "Admin";
        }

        public async Task<IActionResult> Index()
        {
            var rolUsuario = UsuarioEsAdministrador();
            var usuarioId = ObtenerUsuarioIdActual();
            List<Evento> eventos; 
            if (rolUsuario)
            {
                eventos = await _context.Eventos
                    .Include(e => e.Categoria)
                    .Include(e => e.UsuarioRegistro)
                    .ToListAsync();
            }
            else
            {
                eventos = await _context.Eventos
                    .Where(e => e.UsuarioRegistroId == usuarioId)
                    .Include(e => e.Categoria)
                    .ToListAsync();
            }

            return View(eventos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscribirme(int id)
        {
            var usuarioId = ObtenerUsuarioIdActual();

            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evento == null)
            {
                return NotFound();
            }

            var inscripcionesActuales = await _context.InscripcionesEventos
                .CountAsync(i => i.EventoId == id);

            if (inscripcionesActuales >= evento.CupoMaximo)
            {
                TempData["Error"] = "No hay cupo disponible para este evento.";
                return RedirectToAction("Index", "Home");
            }

            var eventosUsuario = await _context.InscripcionesEventos
                .Include(i => i.Evento)
                .Where(i => i.UsuarioId == usuarioId)
                .ToListAsync();

            var mismoDia = eventosUsuario.Any(i =>
                i.Evento.Fecha.Date == evento.Fecha.Date &&
                i.Evento.Hora == evento.Hora 
            );

            if (mismoDia)
            {
                TempData["Error"] = "Ya estás inscrito en otro evento en esa fecha y hora.";
                return RedirectToAction("Index", "Home");
            }

            var inscripcion = new InscripcionEvento
            {
                UsuarioId = usuarioId,
                EventoId = evento.Id,
                FechaInscripcion = DateTime.Now
            };

            _context.InscripcionesEventos.Add(inscripcion);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Te has inscrito correctamente al evento.";
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var usuariosInscritos = await _context.InscripcionesEventos
                .Include(e => e.Usuario)
                .Where(e => e.EventoId == id)
                .ToListAsync();

            return View(usuariosInscritos);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarPresente(int? id)
        {
            var inscripcion = await _context.InscripcionesEventos.FirstOrDefaultAsync(i => i.EventoId == id );

            if (inscripcion == null)
            {
                TempData["Error"] = "El id de la inscripcion no existe en los registros.";
                return RedirectToAction("Details", new { id = inscripcion.EventoId });

            }

            inscripcion.Asistio = true;
            await _context.SaveChangesAsync();

            TempData["Success"] = "El usuario se marco como presente."; 
            return RedirectToAction("Details", new { id = inscripcion.EventoId });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarAusente(int? id)
        {
            var inscripcion = await _context.InscripcionesEventos.FirstOrDefaultAsync(i => i.EventoId == id);

            if (inscripcion == null)
            {
                TempData["Error"] = "El id de la inscripcion no existe en los registros.";
                return RedirectToAction("Details", new { id = inscripcion.EventoId });

            }

            inscripcion.Asistio = false;
            await _context.SaveChangesAsync();

            TempData["Success"] = "El usuario se marco como ausente.";
            return RedirectToAction("Details", new { id = inscripcion.EventoId });

        }

    }
}
