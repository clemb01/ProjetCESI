using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class CreateRessourceViewModel : BaseViewModel
    {
        [Display(Name = "Type de relation")]
        public List<int> SelectedTypeRelation { get; set; }

        [Display(Name = "Categories")]
        public int SelectedCategories { get; set; }

        [Display(Name = "Type de ressource")]
        public int SelectedTypeRessources { get; set; }

        [Display(Name = "Titre")]
        public string Titre { get; set; }
        public string Contenu { get; set; }
    }
}
