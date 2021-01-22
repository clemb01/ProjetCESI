using ProjetCESI.Core;
using ProjetCESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public class TypeRelationRessourceMetier : MetierBase<TypeRelationRessource, TypeRelationRessourceData>, ITypeRelationRessourceMetier
    {
        public async Task AjouterRelationsToRessource(List<int> __listRelations, int __ressourceId)
        {
            if(__listRelations != null && __listRelations.Any())
            {
                IEnumerable<TypeRelationRessource> relations = await DataClass.GetTypeRelationRessourcesByRessourceId(__ressourceId);

                if (relations != null && relations.Any())
                {
                    List<TypeRelationRessource> relationASupprimer = relations.Where(c => !__listRelations.Any(d => d == c.TypeRelationId)).ToList();

                    __listRelations = __listRelations.Where(c => !relations.Any(d => d.TypeRelationId == c)).ToList();

                    await DataClass.Delete(relationASupprimer);
                }

                List<TypeRelationRessource> list = new List<TypeRelationRessource>();

                foreach (var relation in __listRelations)
                {
                    list.Add(new TypeRelationRessource
                    {
                        RessourceId = __ressourceId,
                        TypeRelationId = relation
                    });
                }

                await DataClass.InsertOrUpdate(list);
            }
        }

    }
}
