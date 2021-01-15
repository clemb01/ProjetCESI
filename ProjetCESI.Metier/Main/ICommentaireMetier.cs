using ProjetCESI.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public interface ICommentaireMetier : IMetier<Commentaire>
    {
        Task<IEnumerable<Commentaire>> GetAllCommentairesParentByRessourceId(int __ressourceId);
        Task<Commentaire> GetCommentaireComplet(int __commentaireId);
    }
}