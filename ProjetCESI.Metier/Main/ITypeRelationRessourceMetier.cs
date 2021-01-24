using ProjetCESI.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public interface ITypeRelationRessourceMetier : IMetier<TypeRelationRessource>
    {
        Task AjouterRelationsToRessource(List<int> __listRelations, int __ressourceId);
    }
}