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
    public class UtilisateurRessourceData : Repository<UtilisateurRessource>, IUtilisateurRessourceData
    {
        public async Task<UtilisateurRessource> GetByUtilisateurAndRessourceId(int __utilisateurId, int __ressourceId)
        {
            using(var ctx = GetContext())
            {
                return await ctx.Set<UtilisateurRessource>()
                          .FirstOrDefaultAsync(c => c.UtilisateurId == __utilisateurId && c.RessourceId == __ressourceId);
            }
        }

        public async Task<IEnumerable<TopObject>> GetTopExploitee(int __nombreRecherche)
        {
            try
            {
                using (DbContext ctx = GetContext())
                {
                    DbConnection connection = ctx.Database.GetDbConnection();

                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();

                    var result = new List<TopObject>();

                    using (DbCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT TOP(@count) rs.Titre, COUNT(*) as nombre
                                            FROM [dbo].[UtilisateurRessources] as ur
                                            LEFT OUTER JOIN [dbo].Ressources as rs ON rs.Id = ur.RessourceId
                                            WHERE EstExploite = 1
                                            GROUP BY rs.Titre";

                        var countParam = cmd.CreateParameter();
                        countParam.DbType = DbType.Int32;
                        countParam.ParameterName = "@count";
                        countParam.Value = __nombreRecherche;
                        cmd.Parameters.Add(countParam);

                        DbDataReader reader = await cmd.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            var res = new TopObject
                            {
                                Parametre = reader.GetString(0),
                                Count = reader.GetInt32(1)
                            };

                            result.Add(res);
                        }

                        reader.Close();
                    }

                    connection.Close();

                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Tuple<IEnumerable<Ressource>, IEnumerable<StatutActivite>, int>> GetUserActivite(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
        {
            using (var ctx = GetContext())
            {
                IQueryable<UtilisateurRessource> query = ctx.Set<UtilisateurRessource>()
                        .Include(c => c.Ressource)
                        .ThenInclude(c => c.Categorie)
                        .Include(c => c.Ressource)
                        .ThenInclude(c => c.TypeRessource)
                        .Include(c => c.Ressource)
                        .ThenInclude(c => c.TypeRelationsRessources)
                        .ThenInclude(c => c.TypeRelation)
                        .Include(c => c.Ressource)
                        .ThenInclude(c => c.TypeRelationsRessources)
                        .Where(c => c.Ressource.Statut == Statut.Accepter && c.Ressource.Categorie != null && c.Ressource.RessourceSupprime == false && c.StatutActivite != null && c.UtilisateurId == _userId && c.Ressource.RessourceParent == null);

                if (!string.IsNullOrEmpty(_search))
                    query = Search(query, _search);

                int count = await query.CountAsync();
                int mod = (count % _pagination) != 0 ? 1 : 0;
                count = (count / _pagination) + mod;

                var result = await query.OrderBy(GenerateOrderFilter(_tri))
                        .Skip(_pageOffset * _pagination)
                        .Take(_pagination).ToListAsync();

                return Tuple.Create(result.Select(c => c.Ressource).AsEnumerable(), result.Select(c => c.StatutActivite.GetValueOrDefault()), count);
            }
        }

        private IQueryable<UtilisateurRessource> Search(IQueryable<UtilisateurRessource> query, string search)
        {
            return query.Where(c => c.Ressource.Titre.Contains(search) ||
                                    c.Ressource.Categorie.Nom.Contains(search) ||
                                    c.Ressource.TypeRessource.Nom.Contains(search) ||
                                    c.Ressource.TypeRelationsRessources.Any(c => c.TypeRelation.Nom.Contains(search)));
        }

        private string GenerateOrderFilter(TypeTriBase _tri)
        {
            string tri = "Ressource." + Enum.GetName(_tri);

            if (tri.Contains("Desc"))
                tri = tri.Insert(tri.Length - 4, " ");

            return tri;
        }
    }
}
