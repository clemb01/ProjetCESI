using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class UpdatePasswordViewModel : BaseViewModel
    {


        [Required(ErrorMessage = "Le mot de passe est requis")]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [StringLength(100, ErrorMessage = "Le mot de passe doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau Mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Les deux mots de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; }

    }
}
