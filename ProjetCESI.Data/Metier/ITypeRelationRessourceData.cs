using ProjetCESI.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public interface ITypeRelationRessourceData : IData<TypeRelationRessource>
    {
        Task<IEnumerable<TypeRelationRessource>> GetTypeRelationRessourcesByRessourceId(int __ressourceId);
    }
}