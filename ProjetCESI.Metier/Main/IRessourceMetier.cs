using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public interface IRessourceMetier : IMetier<Ressource>
    {
        Task<Ressource> GetRessourceComplete(int _ressourceId);
        Task<IEnumerable<Ressource>> GetAllPaginedRessource(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0, bool __includeShared = false, bool __includePrivate = false);
        Task<IEnumerable<Ressource>> GetAllPaginedLastRessource(int _pagination = 20, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetAllAdvancedSearchPaginedRessource(string _search, List<int> _categories, List<int> _typeRelation, List<int> _typeRessource, DateTime? _dateDebut, DateTime? _dateFin, TypeTriBase _typeTri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetAllSearchPaginedRessource(string _search, int _pagination = 20, int _pageOffset = 0);
        Task<Tuple<IEnumerable<Ressource>, int>> GetUserFavoriteRessources(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<Tuple<IEnumerable<Ressource>, int>> GetUserRessourcesMiseDeCote(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<Tuple<IEnumerable<Ressource>, int>> GetUserRessourcesExploitee(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<Tuple<IEnumerable<Ressource>, int>> GetUserRessourcesCreees(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<int> InitNewRessource(int __userId);
        Task<IEnumerable<Tuple<int, string, string, List<string>, string, string>>> GetRessourcesAccueil(TypeTriBase _tri = TypeTriBase.DateModificationDesc, int _pagination = 5, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetRessourcesNonValider(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetRessourcesSuspendu(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
    }
}