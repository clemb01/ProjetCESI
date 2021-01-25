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
        public async Task<IEnumerable<Ressource>> GetAllPaginedRessource(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
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
                                 .Where(c => c.Statut == Statut.Accepter)
                                 .OrderBy(GenerateOrderFilter(_tri))
                                 .Skip(_pageOffset * _pagination)
                                 .Take(_pagination);

                return await ressources.ToListAsync();
            }
        }

        public async Task<Ressource> GetRessourceComplete(int _ressourceId)
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
                                 .Include(c => c.Commentaires)
                                 .ThenInclude(c => c.CommentairesEnfant);

                return await ressources.FirstOrDefaultAsync(c => c.Id == _ressourceId);
            }
        }

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetUserRessourcesMiseDeCote(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
        {
            using (var ctx = GetContext())
            {
                IQueryable<Ressource> query = ctx.Set<Ressource>()
                        .Include(c => c.UtilisateurCreateur)
                        .Include(c => c.Categorie)
                        .Include(c => c.TypeRessource)
                        .Include(c => c.TypeRelationsRessources)
                        .ThenInclude(c => c.TypeRelation)
                        .Include(c => c.TypeRelationsRessources)
                        .ThenInclude(c => c.Ressource)
                        .Where(c => c.Statut == Statut.Accepter && c.UtilisateurRessources.Any(a => a.EstMisDeCote && a.UtilisateurId == _userId));

                if(!string.IsNullOrEmpty(_search))
                    query = Search(query, _search);

                int count = await query.CountAsync();
                int mod = (count % _pagination) != 0 ? 1 : 0;
                count = (count / _pagination) + mod;

                var result = await query.OrderBy(GenerateOrderFilter(_tri))
                        .Skip(_pageOffset * _pagination)
                        .Take(_pagination).ToListAsync();

                return Tuple.Create(result.AsEnumerable(), count);
            }
        }

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetUserRessourcesExploitee(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
        {
            using (var ctx = GetContext())
            {
                IQueryable<Ressource> query = ctx.Set<Ressource>()
                        .Include(c => c.UtilisateurCreateur)
                        .Include(c => c.Categorie)
                        .Include(c => c.TypeRessource)
                        .Include(c => c.TypeRelationsRessources)
                        .ThenInclude(c => c.TypeRelation)
                        .Include(c => c.TypeRelationsRessources)
                        .ThenInclude(c => c.Ressource)
                        .Where(c => c.Statut == Statut.Accepter && c.UtilisateurRessources.Any(a => a.EstExploite && a.UtilisateurId == _userId));

                if (!string.IsNullOrEmpty(_search))
                    query = Search(query, _search);

                int count = await query.CountAsync();
                int mod = (count % _pagination) != 0 ? 1 : 0;
                count = (count / _pagination) + mod;

                var result = await query.OrderBy(GenerateOrderFilter(_tri))
                        .Skip(_pageOffset * _pagination)
                        .Take(_pagination).ToListAsync();

                return Tuple.Create(result.AsEnumerable(), count);
            }
        }

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetUserRessourcesCreees(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
        {
            using (var ctx = GetContext())
            {
                IQueryable<Ressource> query = ctx.Set<Ressource>()
                        .Include(c => c.UtilisateurCreateur)
                        .Include(c => c.Categorie)
                        .Include(c => c.TypeRessource)
                        .Include(c => c.TypeRelationsRessources)
                        .ThenInclude(c => c.TypeRelation)
                        .Include(c => c.TypeRelationsRessources)
                        .ThenInclude(c => c.Ressource)
                        .Where(c => c.UtilisateurCreateurId == _userId && c.Statut != Statut.Empty);

                if (!string.IsNullOrEmpty(_search))
                    query = Search(query, _search);

                int count = await query.CountAsync();
                int mod = (count % _pagination) != 0 ? 1 : 0;
                count = (count / _pagination) + mod;

                var result = await query.OrderBy(GenerateOrderFilter(_tri))
                        .Skip(_pageOffset * _pagination)
                        .Take(_pagination).ToListAsync();

                return Tuple.Create(result.AsEnumerable(), count);
            }
        }

        public async Task<IEnumerable<Ressource>> GetRessourcesNonValider(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
        {
            using (var ctx = GetContext())
            {
                return await ctx.Set<Ressource>()
                        .Include(c => c.UtilisateurCreateur)
                        .Include(c => c.Categorie)
                        .Include(c => c.TypeRessource)
                        .Include(c => c.TypeRelationsRessources)
                        .ThenInclude(c => c.TypeRelation)
                        .Include(c => c.TypeRelationsRessources)
                        .ThenInclude(c => c.Ressource)
                        .Where(c => c.Statut == Statut.AttenteValidation)
                        .OrderBy(GenerateOrderFilter(_tri))
                        .Skip(_pageOffset * _pagination)
                        .Take(_pagination).ToListAsync();
            }
        }

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetUserFavoriteRessources(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
        {
            using (var ctx = GetContext())
            {
                IQueryable<Ressource> query = ctx.Set<Ressource>()
                        .Include(c => c.UtilisateurCreateur)
                        .Include(c => c.Categorie)
                        .Include(c => c.TypeRessource)
                        .Include(c => c.TypeRelationsRessources)
                        .ThenInclude(c => c.TypeRelation)
                        .Include(c => c.TypeRelationsRessources)
                        .ThenInclude(c => c.Ressource)
                        .Where(c => c.Statut == Statut.Accepter && c.UtilisateurRessources.Any(a => a.EstFavoris && a.UtilisateurId == _userId));

                if (!string.IsNullOrEmpty(_search))
                    query = Search(query, _search);

                int count = await query.CountAsync();
                int mod = (count % _pagination) != 0 ? 1 : 0;
                count = (count / _pagination) + mod;

                var result = await query.OrderBy(GenerateOrderFilter(_tri))
                        .Skip(_pageOffset * _pagination)
                        .Take(_pagination).ToListAsync();

                return Tuple.Create(result.AsEnumerable(), count);
            }
        }

        private IQueryable<Ressource> Search(IQueryable<Ressource> query, string search)
        {
            return query.Where(c => c.Titre.Contains(search) ||
                                    c.Categorie.Nom.Contains(search) ||
                                    c.TypeRessource.Nom.Contains(search) ||
                                    c.TypeRelationsRessources.Any(c => c.TypeRelation.Nom.Contains(search)));
        }

        public async Task<Ressource> GetFirstEmptyRessource(int __userId)
        {
            using (var ctx = GetContext())
            {
                return await ctx.Set<Ressource>()
                    .FirstOrDefaultAsync(c => c.UtilisateurCreateurId == __userId && c.Statut == Statut.Empty);
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
                                 .Where(c => c.Statut == Statut.Accepter)
                                 .OrderByDescending(c => c.DateModification)
                                 .Skip(_pageOffset * _pagination)
                                 .Take(_pagination);

                return await ressources.ToListAsync();
            }
        }

        public async Task<IEnumerable<Ressource>> GetAllSearchPaginedRessource(string _search, int _pagination = 20, int _pageOffset = 0)
        {
            if (string.IsNullOrEmpty(_search))
                return await GetAllPaginedRessource(_pagination: _pagination, _pageOffset: _pageOffset);

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
                                 .Where(c => c.Statut == Statut.Accepter)
                                 .Where(c => c.Titre.Contains(_search))
                                 .Skip(_pageOffset * _pagination)
                                 .Take(_pagination);

                return await ressources.ToListAsync();
            }
        }

        public async Task<IEnumerable<Ressource>> GetAllAdvancedSearchPaginedRessource(string _search, List<int> _categories, List<int> _typeRelation, List<int> _typeRessource, DateTime? _dateDebut, DateTime? _dateFin, TypeTriBase _typeTri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
        {
            if (string.IsNullOrEmpty(_search))
                return await GetAllPaginedRessource(_pagination: _pagination, _pageOffset: _pageOffset);

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
                                 .Where(c => c.Statut == Statut.Accepter)
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
