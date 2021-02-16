using Microsoft.EntityFrameworkCore;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public class RessourceData : Repository<Ressource>, IRessourceData
    {
        public async Task<Tuple<IEnumerable<Ressource>, int>> GetAllPaginedRessource(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0, bool __includeShared = false, bool __includePrivate = false)
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
                                 .Where(c => c.Statut == Statut.Accepter && c.Categorie != null && !c.RessourceSupprime && c.RessourceParent == null);

                if (__includeShared && !__includePrivate)
                    ressources = ressources.Where(c => c.TypePartage == TypePartage.Public || c.TypePartage == TypePartage.Partage);
                else if (__includePrivate && !__includeShared)
                    ressources = ressources.Where(c => c.TypePartage == TypePartage.Public || c.TypePartage == TypePartage.Prive);
                else if (__includePrivate && __includeShared)
                    ressources = ressources.Where(c => c.TypePartage == TypePartage.Public || c.TypePartage == TypePartage.Partage || c.TypePartage == TypePartage.Prive);
                else
                    ressources = ressources.Where(c => c.TypePartage == TypePartage.Public);

                ressources = ressources.OrderBy(GenerateOrderFilter(_tri));

                int count = await ressources.CountAsync();
                int mod = (count % _pagination) != 0 ? 1 : 0;
                count = (count / _pagination) + mod;

                return Tuple.Create((await ressources.Skip(_pageOffset * _pagination)
                                    .Take(_pagination)
                                    .ToListAsync()).AsEnumerable(), count);
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
                                 .ThenInclude(c => c.Utilisateur)
                                 .Include(c => c.Commentaires)
                                 .ThenInclude(c => c.CommentairesEnfant)
                                 .Include(c => c.Commentaires)
                                 .ThenInclude(c => c.CommentairesEnfant)
                                 .ThenInclude(c => c.Utilisateur);

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
                        .Where(c => c.Statut == Statut.Accepter && c.Categorie != null && !c.RessourceSupprime && c.UtilisateurRessources.Any(a => a.EstMisDeCote && a.UtilisateurId == _userId) && c.RessourceParent == null);

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

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetUserRessourcePrivees(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 20)
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
                        .Where(c => c.TypePartage == TypePartage.Prive && c.UtilisateurCreateurId == _userId && c.RessourceParentId == null && !c.RessourceSupprime);

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
                        .Where(c => c.Statut == Statut.Accepter && c.Categorie != null && !c.RessourceSupprime && c.UtilisateurRessources.Any(a => a.EstExploite && a.UtilisateurId == _userId) && c.RessourceParent == null);

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
                        .Where(c => c.UtilisateurCreateurId == _userId && c.Statut != Statut.Empty && c.RessourceParent == null && !c.RessourceSupprime);

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
                        .Where(c => c.Statut == Statut.AttenteValidation && c.RessourceParent == null)
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
                        .Where(c => c.Statut == Statut.Accepter && c.Categorie != null && !c.RessourceSupprime &&  c.UtilisateurRessources.Any(a => a.EstFavoris && a.UtilisateurId == _userId) && c.RessourceParent == null);

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
                                 .Where(c => c.Statut == Statut.Accepter && c.Categorie != null && !c.RessourceSupprime && c.RessourceParent == null)
                                 .OrderByDescending(c => c.DateModification)
                                 .Skip(_pageOffset * _pagination)
                                 .Take(_pagination);

                return await ressources.ToListAsync();
            }
        }

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetAllSearchPaginedRessource(string _search, int _pagination = 20, int _pageOffset = 0)
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
                                 .Where(c => c.Statut == Statut.Accepter && !c.RessourceSupprime && c.RessourceParent == null && c.Categorie != null && c.TypePartage == TypePartage.Public)
                                 .Where(c => c.Titre.Contains(_search));

                int count = await ressources.CountAsync();
                int mod = (count % _pagination) != 0 ? 1 : 0;
                count = (count / _pagination) + mod;

                return Tuple.Create((await ressources.Skip(_pageOffset * _pagination)
                                 .Take(_pagination).ToListAsync()).AsEnumerable(), count);
            }
        }

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetAllAdvancedSearchPaginedRessource(string _search, List<int> _categories, List<int> _typeRelation, List<int> _typeRessource, DateTime? _dateDebut, DateTime? _dateFin, TypeTriBase _typeTri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
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
                                 .Where(c => c.Statut == Statut.Accepter && !c.RessourceSupprime && c.RessourceParent == null && c.Categorie != null && c.TypePartage == TypePartage.Public)
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

                int count = await ressources.CountAsync();
                int mod = (count % _pagination) != 0 ? 1 : 0;
                count = (count / _pagination) + mod;

                return Tuple.Create((await ressources.Skip(_pageOffset * _pagination)
                                 .Take(_pagination).ToListAsync()).AsEnumerable(), count);
            }
        }

        private string GenerateOrderFilter(TypeTriBase _tri)
        {
            string tri = Enum.GetName(_tri);

            if (tri.Contains("Desc"))
                tri = tri.Insert(tri.Length - 4, " ");

            return tri;
        }

        public async Task<IEnumerable<Ressource>> GetRessourcesSuspendu(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
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
                        .Where(c => c.Statut == Statut.Suspendre && c.RessourceParent == null && !c.RessourceSupprime)
                        .OrderBy(GenerateOrderFilter(_tri))
                        .Skip(_pageOffset * _pagination)
                        .Take(_pagination).ToListAsync();
            }
        }

        public async Task<bool> ResetRessourceStatutWhereCategoryIsNull()
        {
            using (DbContext ctx = GetContext())
            {
                string query = @"UPDATE [dbo].[Ressources]
                                SET Statut = 1
                                WHERE CategorieId IS NULL AND (Statut = 3 OR Statut = 2)";

                int result = await ctx.Database.ExecuteSqlRawAsync(query);

                return result != 0 ? true : false;
            }
        }
    }
}
