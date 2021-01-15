using ProjetCESI.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public interface ITypeRelationRessourceMetier : IMetier<TypeRelationRessource>
    {
        Task AjouterRelationsToRessource(List<int> listRelations, int ressourceId);
    }
}