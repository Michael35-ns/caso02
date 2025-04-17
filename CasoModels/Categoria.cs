using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasoModels
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool Estado { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // FK
        public int UsuarioRegistroId { get; set; }
        public Usuario UsuarioRegistro { get; set; }

        public ICollection<Evento> Eventos { get; set; }
    }
}
