using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class GestionViewModel : BaseViewModel
    {
        public string NomVue { get; set; }
        public List<Ressource> Ressources { get; set; }
        public List<User> Users { get; set; }
        public List<Categorie> categories { get; set; }
        public List<TypeRessource> typeRessources { get; set; }
        public List<TypeRelation> typeRelations { get; set; }
        public string NomListe { get; set; }
        public TopStats TopRecherches { get; set; }
        public TopStats TopConsultations { get; set; }
        public TopStats TopActions { get; set; }
        public TopStats TopExploites { get; set; }
    }

    public class TopStats
    {
        public List<string> Parametres { get; set; }
        public List<int> Count { get; set; }
    }
}
