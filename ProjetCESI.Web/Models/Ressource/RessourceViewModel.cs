using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class RessourceViewModel : BaseViewModel
    {
        public int RessourceId { get; set; }
        public DateTimeOffset DateCreation { get; set; }
        public DateTimeOffset DateModification { get; set; }
        public string Titre { get; set; }
        public string Contenu { get; set; }
        public Statut Statut { get; set; }
        public int NombreConsultation { get; set; }
        public User UtilisateurCreateur { get; set; }
        public TypeRessource TypeRessource { get; set; }
        public Categorie Categorie { get; set; }
        public bool EstFavoris { get; set; }
        public bool EstExploite { get; set; }
        public bool EstMisDeCote { get; set; }
        public StatutActivite? StatutActivite { get; set; }
        public List<TypeRelation> TypeRelations { get; set; }
        public string TypeRelationsString { get; set; }
        public List<Commentaire> Commentaires { get; set; }
        public bool RessourceSupprime { get; set; }
        public DateTimeOffset DateSuppression { get; set; }
        public TypePartage TypePartage { get; set; }
        public string ShareURL { get; set; }
    }
}
