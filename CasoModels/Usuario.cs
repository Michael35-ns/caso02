using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasoModels
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string NombreUsuario { get; set; }

        [Required]
        public string NombreCompleto { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        [Phone]
        public string Telefono { get; set; }

        [Required]
        public string Contrasena { get; set; }

        [Required]
        public string Rol { get; set; }

        public ICollection<Categoria> CategoriasRegistradas { get; set; }
        public ICollection<Evento> EventosRegistrados { get; set; }
    }
}
