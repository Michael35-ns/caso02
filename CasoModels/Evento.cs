using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasoModels
{
    public class Evento
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        // FK Categoría
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Hora { get; set; }

        public string Duracion { get; set; }

        public string Ubicacion { get; set; }

        public int CupoMaximo { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int UsuarioRegistroId { get; set; }
        public Usuario UsuarioRegistro { get; set; }
    }
}
