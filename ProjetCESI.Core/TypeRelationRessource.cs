using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class TypeRelationRessource : EntiteBase
    {
        [DataMember]
        public Ressource Ressource { get; set; }
        [DataMember]
        public int RessourceId { get; set; }
        [DataMember]
        public TypeRelation TypeRelation { get; set; }
        [DataMember]
        public int TypeRelationId { get; set; }
    }
}
