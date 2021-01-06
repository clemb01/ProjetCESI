using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class TypeRelation : EntiteBase
    {
        [DataMember]
        public string NomRelation { get; set; }

        public List<TypeRelationRessource> TypeRelationsRessource { get; set; }
    }
}
