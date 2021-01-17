using ProjetCESI.Core;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public interface IUtilisateurRessourceData : IData<UtilisateurRessource>
    {
        Task<UtilisateurRessource> GetByUtilisateurAndRessourceId(int __utilisateurId, int __ressourceId);
    }
}