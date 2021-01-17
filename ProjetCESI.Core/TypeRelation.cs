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
        public string Nom { get; set; }

        public List<TypeRelationRessource> TypeRelationsRessource { get; set; }
    }

    public enum TypeRelations
    {
        Soi = 1,
        Conjoints,
        Famille,
        Professionnelle,
        Amis,
        Inconnus
    }
}
