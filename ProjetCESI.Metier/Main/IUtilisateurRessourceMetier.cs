using ProjetCESI.Core;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public interface IUtilisateurRessourceMetier : IMetier<UtilisateurRessource>
    {
        Task<UtilisateurRessource> GetByUtilisateurAndRessourceId(int __utilisateurId, int __ressourceId);
        Task<bool> AjouterFavoris(int _utilisateurId, int _ressourceId);
        Task<bool> SupprimerFavoris(int _utilisateurId, int _ressourceId);
        Task<bool> MettreDeCote(int _utilisateurId, int _ressourceId);
        Task<bool> DeMettreDeCote(int _utilisateurId, int _ressourceId);
        Task<bool> EstExploite(int _utilisateurId, int _ressourceId);
        Task<bool> PasExploite(int _utilisateurId, int _ressourceId);
    }
}