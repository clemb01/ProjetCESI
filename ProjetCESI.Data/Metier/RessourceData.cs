using Microsoft.EntityFrameworkCore;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public class RessourceData : Repository<Ressource>, IRessourceData
    {
        public async Task<IEnumerable<Ressource>> GetAllPaginedRessource(int _pagination = 20, int _pageOffset = 0)
        {
            using (var ctx = GetContext())
            {
                var ressources = ctx.Set<Ressource>()
                                 .Include(c => c.UtilisateurCreateur)
                                 .Include(c => c.Categorie)
                                 .Include(c => c.TypeRessource)
                                 .Include(c => c.TypeRelationsRessources)
                                 .Skip(_pageOffset * _pagination)
                                 .Take(_pagination);

                return await ressources.ToListAsync();
            }
        }

        public async Task<IEnumerable<Ressource>> GetAllSearchPaginedRessource(string _search, int _pagination = 20, int _pageOffset = 0)
        {
            if (string.IsNullOrEmpty(_search))
                return await GetAllPaginedRessource(_pagination, _pageOffset);

            using (var ctx = GetContext())
            {
                var ressources = ctx.Set<Ressource>()
                                 .Include(c => c.UtilisateurCreateur)
                                 .Include(c => c.Categorie)
                                 .Include(c => c.TypeRessource)
                                 .Include(c => c.TypeRelationsRessources)
                                 .Skip(_pageOffset * _pagination)
                                 .Take(_pagination);

                return await ressources.ToListAsync();
            }
        }

        public async Task<IEnumerable<Ressource>> GetAllAdvancedSearchPaginedRessource(string _search, List<Categorie> _categories, List<TypeRelation> _typeRelation, List<TypeRessource> _typeRessource, int _pagination = 20, int _pageOffset = 0)
        {
            if (string.IsNullOrEmpty(_search))
                return await GetAllPaginedRessource(_pagination, _pageOffset);

            using (var ctx = GetContext())
            {
                var typeRelationFilter = ctx.Set<TypeRelationRessource>()
                              .Where(c => _typeRelation.Contains(c.TypeRelation));

                var ressources = ctx.Set<Ressource>()
                                 .Include(c => c.UtilisateurCreateur)
                                 .Include(c => c.Categorie)
                                 .Include(c => c.TypeRessource)
                                 .Include(c => c.TypeRelationsRessources);

                if (_categories != null && _categories.Any())
                    ressources.Where(c => _categories.Contains(c.Categorie));

                if (_typeRessource != null && _typeRessource.Any())
                    ressources.Where(c => _typeRessource.Contains(c.TypeRessource));

                if (_typeRelation != null && _typeRelation.Any())
                    ressources.Where(c => c.TypeRelationsRessources.Intersect(typeRelationFilter) != null && c.TypeRelationsRessources.Intersect(typeRelationFilter).Count() > 0);

                    return await ressources.Skip(_pageOffset * _pagination)
                                 .Take(_pagination).ToListAsync();
            }
        }
    }
}
