using System.ComponentModel.DataAnnotations;

namespace CasoMVC.Models
{
    public class UsuarioViewModel
    {
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
    }
}
