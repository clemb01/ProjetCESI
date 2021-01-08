using ProjetCESI.Core;
using ProjetCESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public class RessourceMetier : MetierBase<Ressource, RessourceData>, IRessourceMetier
    {
        public async Task<IEnumerable<Ressource>> GetAllPaginedRessource(int _pagination = 20, int _pageOffset = 0) => await DataClass.GetAllPaginedRessource(_pagination, _pageOffset);

        public async Task<IEnumerable<Ressource>> GetAllAdvancedSearchPaginedRessource(string _search, List<Categorie> _categories, List<TypeRelation> _typeRelation, List<TypeRessource> _typeRessource, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetAllAdvancedSearchPaginedRessource(_search, _categories, _typeRelation, _typeRessource, _pagination, _pageOffset);

        public async Task<IEnumerable<Ressource>> GetAllSearchPaginedRessource(string _search, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetAllSearchPaginedRessource(_search, _pagination, _pageOffset);
    }
}
