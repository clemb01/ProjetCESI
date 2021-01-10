using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class RechercheRessourceViewModel : BaseViewModel
    {
        [Required]
        [Display(Name = "Terme de la recherche")]
        public string Recherche { get; set; }

        [Display(Name = "Type de relation")]
        public List<int> SelectedTypeRelation { get; set; }

        [Display(Name = "Categories")]
        public List<int> SelectedCategories { get; set; }

        [Display(Name = "Type de ressource")]
        public List<int> SelectedTypeRessources { get; set; }

        [Display(Name = "Entre le")]
        public DateTime? DateDebut { get; set; }

        [Display(Name = "Et le")]
        public DateTime? DateFin { get; set; }

        public int TypeTri { get; set; }

        public int Page { get; set; }

        public ListRessourceViewModel Ressources { get; set; } = new ListRessourceViewModel();
    }
}
