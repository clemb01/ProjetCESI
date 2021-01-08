using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class Ressource : EntiteBase
    {
        [DataMember]
        public DateTimeOffset DateCreation { get; set; }
        [DataMember]
        public DateTimeOffset DateModification { get; set; }
        [DataMember]
        public string Titre { get; set; }
        [DataMember]
        public string Contenu { get; set; }
        [DataMember]
        public bool EstValide { get; set; }
        [DataMember]
        public int NombreConsultation { get; set; }

        [DataMember]
        public User UtilisateurCreateur { get; set; }
        [DataMember]
        public int UtilisateurCreateurId { get; set; }
        [DataMember]
        public TypeRessource TypeRessource { get; set; }
        [DataMember]
        public int TypeRessourceId { get; set; }
        [DataMember]
        public Categorie Categorie { get; set; }
        [DataMember]
        public int CategorieId { get; set; }

        public List<UtilisateurRessource> UtilisateurRessources { get; set; }
        public List<TypeRelationRessource> TypeRelationsRessources { get; set; }
        public List<Commentaire> Commentaires { get; set; }
    }
}