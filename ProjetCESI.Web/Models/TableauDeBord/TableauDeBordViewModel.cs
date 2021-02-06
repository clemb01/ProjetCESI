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

        public List<RessourceTableauBord> Ressources { get; set; }
    }

    public class RessourceTableauBord
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public Statut Statut { get; set; }
        public StatutActivite? StatutActivite { get; set; }
        public Categorie Categorie { get; set; }
        public TypeRessource TypeRessource { get; set; }
        public List<TypeRelationRessource> TypeRelationsRessources { get; set; }
    }
}
