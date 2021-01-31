using Microsoft.EntityFrameworkCore;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
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
                                            FROM [projetCESI].[dbo].[UtilisateurRessources] as ur
                                            LEFT OUTER JOIN [projetCESI].[dbo].Ressources as rs ON rs.Id = ur.RessourceId
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
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
