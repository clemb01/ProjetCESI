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
        Task<IEnumerable<Ressource>> GetUserFavoriteRessources(int _userId, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetUserRessourcesMiseDeCote(int _userId, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetUserRessourcesExploitee(int _userId, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetUserRessourcesCreees(int _userId, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0);
        Task<Ressource> GetFirstEmptyRessource(int __userId);
    }
}