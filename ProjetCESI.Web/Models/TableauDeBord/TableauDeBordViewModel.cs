using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class TableauDeBordViewModel : BaseViewModel
    {
        public string NomVue { get; set; }

        public string Tri { get; set; }

        public int Page { get; set; }

        public int NombrePages { get; set; }
        public string Recherche { get; set; }

        public List<Ressource> Ressources { get; set; }
    }
}
