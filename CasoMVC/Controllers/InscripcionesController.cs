using Azure;
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
        public async Task<IActionResult> Inscribirme(int id)
        {
            var usuarioId = ObtenerUsuarioIdActual();

            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evento == null)
            {
                return Json(new { success = false, message = "Evento no encontrado." });
            }

            var inscripcionesActuales = await _context.InscripcionesEventos
                .CountAsync(i => i.EventoId == id);

            if (inscripcionesActuales >= evento.CupoMaximo)
            {
                return Json(new { success = false, message = "No hay cupo disponible para este evento." });
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
                return Json(new { success = false, message = "Ya estás inscrito en otro evento en esa fecha y hora." });
            }

            var inscripcion = new InscripcionEvento
            {
                UsuarioId = usuarioId,
                EventoId = evento.Id,
                FechaInscripcion = DateTime.Now
            };

            _context.InscripcionesEventos.Add(inscripcion);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
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
            var inscripcion = await _context.InscripcionesEventos.FindAsync(id);

            if (inscripcion == null)
            {
                TempData["Error"] = "El id de la inscripcion no existe en los registros.";
                return RedirectToAction("Details", new { id = id });

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
            var inscripcion = await _context.InscripcionesEventos.FindAsync(id);

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
