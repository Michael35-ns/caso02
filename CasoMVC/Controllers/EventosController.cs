using CasoModels;
using CasoMVC.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CasoMVC.Controllers
{
    public class EventosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventoService _eventoService;

        public EventosController(ApplicationDbContext context, IEventoService eventoService)
        {
            _context = context;
            _eventoService = eventoService;

        }

        public async Task<IActionResult> Index()
        {
            var eventos = _context.Eventos.Include(e => e.UsuarioRegistro);
            return View(await eventos.ToListAsync());
        }


        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Descripcion,CategoriaId,Fecha,Hora,Duracion,Ubicacion,CupoMaximo")] Evento evento)
        {
            evento.FechaRegistro = DateTime.Now;

            if (int.TryParse(HttpContext.Session.GetString("UsuarioId"), out int idUsuario))
            {
                evento.UsuarioRegistroId = idUsuario;
            }
            else
            {
                ModelState.AddModelError("", "No se pudo obtener el ID del usuario desde la sesión.");
            }

            if (evento.Fecha < DateTime.Today)
                ModelState.AddModelError("Fecha", "La fecha no puede estar en el pasado.");

            if (!int.TryParse(evento.Duracion, out int duracion) || duracion <= 0)
                ModelState.AddModelError("Duracion", "La duración debe ser un número positivo.");

            if (evento.CupoMaximo <= 0)
                ModelState.AddModelError("CupoMaximo", "El cupo máximo debe ser mayor a 0.");
            ModelState.Remove("UsuarioRegistro");
            ModelState.Remove("Categoria");

            if (ModelState.IsValid)
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", evento.CategoriaId);
            return View(evento);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", evento.CategoriaId);

            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descripcion,CategoriaId,Fecha,Hora,Duracion,Ubicacion,CupoMaximo")] Evento evento)
        {
            if (id != evento.Id)
            {
                return NotFound();
            }

            evento.FechaRegistro = DateTime.Now;

            if (int.TryParse(HttpContext.Session.GetString("UsuarioId"), out int idUsuario))
            {
                evento.UsuarioRegistroId = idUsuario;
            }
            else
            {
                ModelState.AddModelError("", "No se pudo obtener el ID del usuario desde la sesión.");
            }

            if (evento.Fecha < DateTime.Today)
                ModelState.AddModelError("Fecha", "La fecha no puede estar en el pasado.");

            if (!int.TryParse(evento.Duracion, out int duracion) || duracion <= 0)
                ModelState.AddModelError("Duracion", "La duración debe ser un número positivo.");

            if (evento.CupoMaximo <= 0)
                ModelState.AddModelError("CupoMaximo", "El cupo máximo debe ser mayor a 0.");

            ModelState.Remove("UsuarioRegistro");
            ModelState.Remove("Categoria");

            if (ModelState.IsValid)
            {
                _context.Update(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", evento.CategoriaId);
            return View(evento);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.UsuarioRegistro)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.UsuarioRegistro)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Eventos()
        {
            var eventos = await _eventoService.ObtenerEventosAsync();
            return View("Eventos", eventos); // Vista: Views/EventosPublicos/Eventos.cshtml
        }

        // GET: /EventosPublicos/EventoDetails/5
        [HttpGet]
        public async Task<IActionResult> EventoDetails(int id)
        {
            var evento = await _eventoService.ObtenerEventoPorIdAsync(id);

            if (evento == null)
                return NotFound();

            return View("EventoDetails", evento); 
        }
    }


}
