using System.ComponentModel.DataAnnotations;

namespace BackendGymStar.DTO.Entradas
{
    public class UserLogin
    {
        [Display(Name = "Email"), Required(ErrorMessage = "El Campo {0} es requerido")]
        public string? Email { get; set; }

        [Display(Name = "Password"), Required(ErrorMessage = "El Campo {0} es requerido")]
        public string? Password { get; set; }
    }
}
