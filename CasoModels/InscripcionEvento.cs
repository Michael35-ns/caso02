using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasoModels
{
    public class InscripcionEvento
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int EventoId { get; set; }
        public Evento Evento { get; set; }

        public DateTime FechaInscripcion { get; set; } = DateTime.Now;
        public bool Asistio { get; set; } = false;
    }
}
