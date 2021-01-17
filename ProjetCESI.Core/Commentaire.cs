using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class Commentaire : EntiteBase
    {
        [DataMember]
        public DateTimeOffset DateCreation { get; set; }
        [DataMember]
        public DateTimeOffset DateModification { get; set; }
        [DataMember]
        public string Texte { get; set; }


        [DataMember]
        public User Utilisateur { get; set; }
        [DataMember]
        public int? UtilisateurId { get; set; }
        [DataMember]
        public Ressource Ressource { get; set; }
        [DataMember]
        public int RessourceId { get; set; }
        [DataMember]
        public Commentaire CommentaireParent { get; set; }
        [DataMember]
        public int? CommentaireParentId { get; set; }

        public List<Commentaire> CommentairesEnfant { get; set; }
    }
}
