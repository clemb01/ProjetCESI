using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class Statistique : EntiteBase
    {
        [DataMember]
        public string RechercheEffectue { get; set; }

        [DataMember]
        public string ParametreRecherche { get; set; }

        [DataMember]
        public DateTimeOffset DateRecherche { get; set; }

        [DataMember]
        public User Utilisateur { get; set; }

        [DataMember]
        public int? UtilisateurId { get; set; }
    }
}
