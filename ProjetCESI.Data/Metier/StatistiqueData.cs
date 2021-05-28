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
    public class StatistiqueData : Repository<Statistique>, IStatistiqueData
    {
        public async Task<IEnumerable<TopActions>> GetNombreActionsMoyenneParUtilisateurs(TimestampFilter __filter, DateTimeOffset __whereBas, DateTimeOffset __whereHaut)
        {
            try
            {
                using (DbContext ctx = GetContext())
                {
                    DbConnection connection = ctx.Database.GetDbConnection();

                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();

                    var result = new List<TopActions>();

                    using (DbCommand cmd = connection.CreateCommand())
                    {
                        if (__filter == TimestampFilter.Day)
                        {
                            cmd.CommandText = @"SELECT dateadd(HOUR, datediff(HOUR, 0, DateRecherche), 0) as TimeStampHour, Count(*) as 'Nombre actions'
		                                    FROM [dbo].[Statistiques] 
		                                    WHERE [DateRecherche] <= @whereHaut AND [DateRecherche] >= @whereBas
		                                    GROUP BY dateadd(HOUR, datediff(HOUR, 0, DateRecherche), 0)
                                            ORDER BY dateadd(HOUR, datediff(HOUR, 0, DateRecherche), 0);";
                        }
                        else if (__filter == TimestampFilter.Week)
                        {
                            cmd.CommandText = @"SELECT dateadd(DAY, datediff(DAY, 0, DateRecherche), 0) as TimeStampHour, Count(*) as 'Nombre actions'
		                                    FROM [dbo].[Statistiques] 
		                                    WHERE [DateRecherche] <= @whereHaut AND [DateRecherche] >= @whereBas
		                                    GROUP BY dateadd(DAY, datediff(DAY, 0, DateRecherche), 0)
                                            ORDER BY dateadd(DAY, datediff(DAY, 0, DateRecherche), 0);";
                        }
                        else if (__filter == TimestampFilter.Month)
                        {
                            cmd.CommandText = @"SELECT dateadd(DAY, datediff(DAY, 0, DateRecherche), 0) as TimeStampHour, Count(*) as 'Nombre actions'
		                                    FROM [dbo].[Statistiques] 
		                                    WHERE [DateRecherche] <= @whereHaut AND [DateRecherche] >= @whereBas
		                                    GROUP BY dateadd(DAY, datediff(DAY, 0, DateRecherche), 0)
                                            ORDER BY dateadd(DAY, datediff(DAY, 0, DateRecherche), 0);";
                        }
                        else if (__filter == TimestampFilter.Year)
                        {
                            cmd.CommandText = @"SELECT dateadd(MONTH, datediff(MONTH, 0, DateRecherche), 0) as TimeStampHour, Count(*) as 'Nombre actions'
		                                    FROM [dbo].[Statistiques] 
		                                    WHERE [DateRecherche] <= @whereHaut AND [DateRecherche] >= @whereBas
		                                    GROUP BY dateadd(MONTH, datediff(MONTH, 0, DateRecherche), 0)
                                            ORDER BY dateadd(MONTH, datediff(MONTH, 0, DateRecherche), 0);";
                        }
                        else
                        {
                            cmd.CommandText = @"SELECT dateadd(DAY, datediff(DAY, 0, DateRecherche), 0) as TimeStampHour, Count(*) as 'Nombre actions'
		                                    FROM [dbo].[Statistiques] 
		                                    WHERE [DateRecherche] <= @whereHaut AND [DateRecherche] >= @whereBas
		                                    GROUP BY dateadd(DAY, datediff(DAY, 0, DateRecherche), 0)
                                            ORDER BY dateadd(DAY, datediff(DAY, 0, DateRecherche), 0);";
                        }

                        var whereHautParam = cmd.CreateParameter();
                        whereHautParam.ParameterName = "@whereHaut";
                        whereHautParam.Value = __whereHaut.ToString("O");
                        cmd.Parameters.Add(whereHautParam);

                        var whereBasParam = cmd.CreateParameter();
                        whereBasParam.ParameterName = "@whereBas";
                        whereBasParam.Value = __whereBas.ToString("O");
                        cmd.Parameters.Add(whereBasParam);

                        DbDataReader reader = await cmd.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            var res = new TopActions
                            {
                                Date = reader.GetDateTime(0),
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

        public async Task<IEnumerable<TopObject>> GetTopConsultation(int __nbRecherche, DateTimeOffset __whereBas, DateTimeOffset __whereHaut, int __offset = 0)
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
                        cmd.CommandText = @"SELECT Parametre, count(Parametre) as 'Nombre de fois consulté'
                                            FROM [dbo].[Statistiques] 
                                            WHERE Action = 'Ressource' AND
                                                [DateRecherche] <= @whereHaut AND [DateRecherche] >= @whereBas AND
                                                Parametre IS NOT NULL
                                            GROUP BY Parametre
                                            ORDER BY 'Nombre de fois consulté' Desc
                                            OFFSET @offset ROWS
                                            FETCH NEXT @count ROWS ONLY";

                        var countParam = cmd.CreateParameter();
                        countParam.DbType = DbType.Int32;
                        countParam.ParameterName = "@count";
                        countParam.Value = __nbRecherche;
                        cmd.Parameters.Add(countParam);

                        var offsetParam = cmd.CreateParameter();
                        offsetParam.DbType = DbType.Int32;
                        offsetParam.ParameterName = "@offset";
                        offsetParam.Value = __offset;
                        cmd.Parameters.Add(offsetParam);

                        var whereHautParam = cmd.CreateParameter();
                        whereHautParam.ParameterName = "@whereHaut";
                        whereHautParam.Value = __whereHaut.ToString("O");
                        cmd.Parameters.Add(whereHautParam);

                        var whereBasParam = cmd.CreateParameter();
                        whereBasParam.ParameterName = "@whereBas";
                        whereBasParam.Value = __whereBas.ToString("O");
                        cmd.Parameters.Add(whereBasParam);

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

        public async Task<IEnumerable<TopObject>> GetTopRecherche(int __nbRecherche, DateTimeOffset __whereBas, DateTimeOffset __whereHaut, int __offset = 0)
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
                        cmd.CommandText = @"SELECT Parametre, count(Parametre) as 'Nombre de fois recherché'
                                            FROM [dbo].[Statistiques] 
                                            WHERE Action = 'Search' AND
                                                [DateRecherche] <= @whereHaut AND [DateRecherche] >= @whereBas
                                            GROUP BY Parametre
                                            ORDER BY 'Nombre de fois recherché' Desc
                                            OFFSET @offset ROWS
                                            FETCH NEXT @count ROWS ONLY";

                        var countParam = cmd.CreateParameter();
                        countParam.DbType = DbType.Int32;
                        countParam.ParameterName = "@count";
                        countParam.Value = __nbRecherche;
                        cmd.Parameters.Add(countParam);

                        var offsetParam = cmd.CreateParameter();
                        offsetParam.DbType = DbType.Int32;
                        offsetParam.ParameterName = "@offset";
                        offsetParam.Value = __offset;
                        cmd.Parameters.Add(offsetParam);

                        var whereHautParam = cmd.CreateParameter();
                        whereHautParam.ParameterName = "@whereHaut";
                        whereHautParam.Value = __whereHaut.ToString("O");
                        cmd.Parameters.Add(whereHautParam);

                        var whereBasParam = cmd.CreateParameter();
                        whereBasParam.ParameterName = "@whereBas";
                        whereBasParam.Value = __whereBas.ToString("O");
                        cmd.Parameters.Add(whereBasParam);

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
    }

    public enum TimestampFilter
    {
        Day,
        Week,
        Month,
        Year
    }

    public class TopActions
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class TopObject
    {
        public string Parametre { get; set; }
        public int Count { get; set; }
    }
}
