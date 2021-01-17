using ProjetCESI.Core;
using ProjetCESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public class TypeRelationData : Repository<TypeRelation>, ITypeRelationData
    {
        public override async Task<IEnumerable<TypeRelation>> CreationDonneesTable()
        {
            List<TypeRelation> listeRelation = new List<TypeRelation>();

            listeRelation.Add(new TypeRelation { Nom = "Soi" });
            listeRelation.Add(new TypeRelation { Nom = "Conjoints" });
            listeRelation.Add(new TypeRelation { Nom = "Famille : enfants / parent / fratrie" });
            listeRelation.Add(new TypeRelation { Nom = "Professionnelle : collègues, collaborateur et managers" });
            listeRelation.Add(new TypeRelation { Nom = "Amis et communautés" });
            listeRelation.Add(new TypeRelation { Nom = "Inconnus" });

            await InsertOrUpdate(listeRelation);

            return listeRelation;
        }
    }
}
