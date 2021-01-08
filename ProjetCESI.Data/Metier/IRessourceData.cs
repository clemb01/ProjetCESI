using ProjetCESI.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public interface IRessourceData
    {
        Task<IEnumerable<Ressource>> GetAllPaginedRessource(int pagination = 20, int pageOffset = 0);
        Task<IEnumerable<Ressource>> GetAllAdvancedSearchPaginedRessource(string _search, List<Categorie> _categories, List<TypeRelation> _typeRelation, List<TypeRessource> _typeRessource, int _pagination = 20, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetAllSearchPaginedRessource(string _search, int _pagination = 20, int _pageOffset = 0);
    }
}