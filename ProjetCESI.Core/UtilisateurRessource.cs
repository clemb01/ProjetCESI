using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class UtilisateurRessource : EntiteBase
    {
        [DataMember]
        public bool EstFavoris { get; set; }
        [DataMember]
        public bool EstExploite { get; set; }
        [DataMember]
        public bool EstMisDeCote { get; set; }

        [DataMember]
        public User Utilisateur { get; set; }
        [DataMember]
        public int UtilisateurId { get; set; }
        [DataMember]
        public Ressource Ressource { get; set; }
        [DataMember]
        public int RessourceId { get; set; }
    }
}
