using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public interface IRessourceData : IData<Ressource>
    {
        Task<Ressource> GetRessourceComplete(int _ressourceId);
        Task<IEnumerable<Ressource>> GetAllPaginedRessource(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetAllPaginedLastRessource(int _pagination = 20, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetAllAdvancedSearchPaginedRessource(string _search, List<int> _categories, List<int> _typeRelation, List<int> _typeRessource, DateTime? _dateDebut, DateTime? _dateFin, TypeTriBase _typeTri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetAllSearchPaginedRessource(string _search, int _pagination = 20, int _pageOffset = 0);
        Task<Tuple<IEnumerable<Ressource>, int>> GetUserFavoriteRessources(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<Tuple<IEnumerable<Ressource>, int>> GetUserRessourcesMiseDeCote(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<Tuple<IEnumerable<Ressource>, int>> GetUserRessourcesExploitee(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<Tuple<IEnumerable<Ressource>, int>> GetUserRessourcesCreees(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<Ressource> GetFirstEmptyRessource(int __userId);
        Task<IEnumerable<Ressource>> GetRessourcesNonValider(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        
    }
}