using ProjetCESI.Core;
using ProjetCESI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public interface IUtilisateurRessourceMetier : IMetier<UtilisateurRessource>
    {
        Task<UtilisateurRessource> GetByUtilisateurAndRessourceId(int __utilisateurId, int __ressourceId, bool _flagIsActivite);
        Task<bool> AjouterFavoris(int _utilisateurId, int _ressourceId);
        Task<bool> SupprimerFavoris(int _utilisateurId, int _ressourceId);
        Task<bool> MettreDeCote(int _utilisateurId, int _ressourceId);
        Task<bool> DeMettreDeCote(int _utilisateurId, int _ressourceId);
        Task<bool> EstExploite(int _utilisateurId, int _ressourceId);
        Task<bool> PasExploite(int _utilisateurId, int _ressourceId);
        Task<bool> DemarrerActivite(int _utilisateurId, int _ressourceId);
        Task<bool> SuspendreActivite(int _utilisateurId, int _ressourceId);
        Task<bool> ReprendreActivite(int _utilisateurId, int _ressourceId);
        Task<bool> QuitterActivite(int _utilisateurId, int _ressourceId);
        Task<bool> TerminerActivite(int _utilisateurId, int _ressourceId);
        Task<IEnumerable<TopObject>> GetTopExploitee(int __nombreRecherche);
    }
}