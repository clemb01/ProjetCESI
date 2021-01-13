using ProjetCESI.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public interface ICommentaireData : IData<Commentaire>
    {
        Task<IEnumerable<Commentaire>> GetAllCommentairesParentByRessourceId(int __ressourceId);
    }
}