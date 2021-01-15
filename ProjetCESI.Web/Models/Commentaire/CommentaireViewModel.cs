using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class CommentaireViewModel : BaseViewModel
    {
        public DateTimeOffset DateCreation { get; set; }
        public DateTimeOffset DateModification { get; set; }
        public string Contenu { get; set; }
        public int UtilisateurId { get; set; }
        public int RessourceId { get; set; }
        public Commentaire CommentaireParent { get; set; }
        public int? CommentaireParentId { get; set; }
        public List<CommentaireViewModel> CommentairesEnfant { get; set; }
    }

    public class CommentairesViewModel : BaseViewModel
    {
        public int RessourceId { get; set; }
        public List<Commentaire> Commentaires { get; set; }
    }
}
