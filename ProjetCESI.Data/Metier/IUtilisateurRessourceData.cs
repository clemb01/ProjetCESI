using ProjetCESI.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public interface IUtilisateurRessourceData : IData<UtilisateurRessource>
    {
        Task<UtilisateurRessource> GetByUtilisateurAndRessourceId(int __utilisateurId, int __ressourceId);
        Task<IEnumerable<TopObject>> GetTopExploitee(int __nombreRecherche);
    }
}