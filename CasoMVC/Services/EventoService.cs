using System.Net.Http;
using System.Net.Http.Json;
using CasoMVC.Interface;
using CasoMVC.Models;

namespace CasoMVC.Services
{
    public class EventoService : IEventoService
    {
        private readonly HttpClient _httpClient;

        public EventoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");

        }

        public async Task<List<EventoViewModel>> ObtenerEventosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<EventoViewModel>>("api/events")
                   ?? new List<EventoViewModel>();
        }

        public async Task<EventoViewModel?> ObtenerEventoPorIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<EventoViewModel>($"api/events/{id}");
        }
    }
}
