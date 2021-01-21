using Microsoft.AspNetCore.Http;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class CreateRessourceViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Vous devez selectionner au moins un type de relation")]
        [Display(Name = "Type de relation")]
        public List<int> SelectedTypeRelation { get; set; }

        [Required(ErrorMessage = "Vous devez selectionner une catégorie")]
        [Display(Name = "Categories")]
        public int SelectedCategories { get; set; }

        [Required(ErrorMessage = "Vous devez selectionner un type de ressource")]
        [Display(Name = "Type de ressource")]
        public int SelectedTypeRessources { get; set; }

        [Required(ErrorMessage = "Le titre est requis")]
        [Display(Name = "Titre")]
        public string Titre { get; set; }

        [Required]
        public TypePartage TypePartage { get; set; }

        public string Contenu { get; set; }
        public IFormFile File { get; set; }
        public string urlVideo { get; set; }
    }
}
