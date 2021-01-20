using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class AccueilViewModel : BaseViewModel
    {
        public List<RessourceAccueil> RessourcesPlusVues { get; set; }
        public List<RessourceAccueil> RessourcesPlusRecentes { get; set; }

    }

    public class RessourceAccueil
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Apercu { get; set; }
        public string Categorie { get; set; }
        public string TypeRessource { get; set; }
        public List<string> TypeRelations { get; set; }

    }
}
