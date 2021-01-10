using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public interface ITypeRelationRessourceMetier
    {
        Task AjouterRelationsToRessource(List<int> listRelations, int ressourceId);
    }
}