using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class LoginViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Votre identifiant est requis")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Votre identifiant n'est pas valide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }
    }
}
