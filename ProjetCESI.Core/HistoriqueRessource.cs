using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class HistoriqueRessource : EntiteBase
    {
        [DataMember]
        public DateTimeOffset DateCreation { get; set; }
        [DataMember]
        public Ressource Ressource { get; set; }
        [DataMember]
        public int? RessourceId { get; set; }
        [DataMember]
        public string Titre { get; set; }
        [DataMember]
        public string Contenu { get; set; }
        [DataMember]
        public TypeRessource TypeRessource { get; set; }
        [DataMember]
        public int TypeRessourceId { get; set; }
        [DataMember]
        public Categorie Categorie { get; set; }
        [DataMember]
        public int CategorieId { get; set; }
        [DataMember]
        public string TypeRelationSerializer { get; set; }
    }
}
