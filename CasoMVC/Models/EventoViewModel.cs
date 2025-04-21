namespace CasoMVC.Models
{
    public class EventoViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public int CupoMaximo { get; set; }
        public string Categoria { get; set; }
    }
}
