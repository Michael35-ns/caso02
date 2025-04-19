using System.ComponentModel.DataAnnotations;

namespace CasoMVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe ingresar el nombre de usuario")]
        [Display(Name = "Nombre de usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }
    }
}
