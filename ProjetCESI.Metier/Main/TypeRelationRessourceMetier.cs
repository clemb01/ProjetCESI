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
        public async Task AjouterRelationsToRessource(List<int> listRelations, int ressourceId)
        {
            List<TypeRelationRessource> list = new List<TypeRelationRessource>();

            foreach (var relation in listRelations)
            {
                list.Add(new TypeRelationRessource
                {
                    RessourceId = ressourceId,
                    TypeRelationId = relation
                }) ;
            }

            await DataClass.InsertOrUpdate(list);
        }

    }
}
