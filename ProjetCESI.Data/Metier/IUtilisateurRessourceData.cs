using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public interface IUtilisateurRessourceData : IData<UtilisateurRessource>
    {
        Task<UtilisateurRessource> GetByUtilisateurAndRessourceId(int __utilisateurId, int __ressourceId);
        Task<IEnumerable<TopObject>> GetTopExploitee(int __nombreRecherche);
        Task<Tuple<IEnumerable<Ressource>, IEnumerable<StatutActivite>, int>> GetUserActivite(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
    }
}