using CasoMVC.Models;

namespace CasoMVC.Interface
{
    public interface IEventoService
    {
        Task<List<EventoViewModel>> ObtenerEventosAsync();
        Task<EventoViewModel?> ObtenerEventoPorIdAsync(int id);
    }
}
