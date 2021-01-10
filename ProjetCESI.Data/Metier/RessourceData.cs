using Microsoft.EntityFrameworkCore;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
                                 .ThenInclude(c => c.TypeRelation)
                                 .Include(c => c.TypeRelationsRessources)
                                 .ThenInclude(c => c.Ressource)
                                 .Skip(_pageOffset * _pagination)
                                 .Take(_pagination);

                return await ressources.ToListAsync();
            }
        }

        public async Task<IEnumerable<Ressource>> GetAllPaginedLastRessource(int _pagination = 20, int _pageOffset = 0)
        {
            using (var ctx = GetContext())
            {
                var ressources = ctx.Set<Ressource>()
                                 .Include(c => c.UtilisateurCreateur)
                                 .Include(c => c.Categorie)
                                 .Include(c => c.TypeRessource)
                                 .Include(c => c.TypeRelationsRessources)
                                 .ThenInclude(c => c.TypeRelation)
                                 .Include(c => c.TypeRelationsRessources)
                                 .ThenInclude(c => c.Ressource)
                                 .OrderByDescending(c => c.DateModification)
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
                                 .ThenInclude(c => c.TypeRelation)
                                 .Include(c => c.TypeRelationsRessources)
                                 .ThenInclude(c => c.Ressource)
                                 .Where(c => c.Titre.Contains(_search))
                                 .Skip(_pageOffset * _pagination)
                                 .Take(_pagination);

                return await ressources.ToListAsync();
            }
        }

        public async Task<IEnumerable<Ressource>> GetAllAdvancedSearchPaginedRessource(string _search, List<int> _categories, List<int> _typeRelation, List<int> _typeRessource, DateTime? _dateDebut, DateTime? _dateFin, TypeTriBase _typeTri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
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
                                 .ThenInclude(c => c.TypeRelation)
                                 .Include(c => c.TypeRelationsRessources)
                                 .ThenInclude(c => c.Ressource)
                                 .Where(c => c.Titre.Contains(_search));

                if (_categories != null && _categories.Any())
                    ressources = ressources.Where(c => _categories.Contains(c.Categorie.Id));

                if (_typeRessource != null && _typeRessource.Any())
                    ressources = ressources.Where(c => _typeRessource.Contains(c.TypeRessource.Id));

                if (_typeRelation != null && _typeRelation.Any())
                {
                    var typeRelationFilter = ctx.Set<TypeRelationRessource>()
                                  .Where(c => _typeRelation.Contains(c.TypeRelation.Id));

                    ressources = ressources.Where(c => c.TypeRelationsRessources.Any(c => typeRelationFilter.Contains(c)));
                }

                if(_dateDebut.HasValue)
                    ressources = ressources.Where(c => c.DateModification > _dateDebut.Value);

                if (_dateFin.HasValue)
                    ressources = ressources.Where(c => c.DateModification < _dateFin.Value);

                ressources = ressources.OrderBy(GenerateOrderFilter(_typeTri));

                return await ressources.Skip(_pageOffset * _pagination)
                                 .Take(_pagination).ToListAsync();
            }
        }

        private string GenerateOrderFilter(TypeTriBase _tri)
        {
            string tri = Enum.GetName(_tri);

            if (tri.Contains("Desc"))
                tri = tri.Insert(tri.Length - 4, " ");

            return tri;
        }
    }
}
