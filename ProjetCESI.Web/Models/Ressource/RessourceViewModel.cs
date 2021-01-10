using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models.Ressource
{
    public class RessourceViewModel
    {
        public DateTimeOffset DateCreation { get; set; }
        public DateTimeOffset DateModification { get; set; }
        public string Titre { get; set; }
        public string Contenu { get; set; }
        public User UtilisateurCreateur { get; set; }
        public bool EstValide { get; set; } = false;
        public TypeRessource TypeRessource { get; set; }
        public Categorie Categorie { get; set; }
        public int NombreConsultation { get; set; }

        public List<UtilisateurRessource> UtilisateurRessources { get; set; }
        public List<TypeRelationRessource> TypeRelationsRessources { get; set; }
        public List<Commentaire> Commentaires { get; set; }

    }
}
