using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class ListRessourceViewModel : BaseViewModel
    {
        public int Page { get; set; } = 1;
        public int NombrePages { get; set; }
        public int TypeTri { get; set; }
        public List<Ressource> Ressources { get; set; }
    }
}
