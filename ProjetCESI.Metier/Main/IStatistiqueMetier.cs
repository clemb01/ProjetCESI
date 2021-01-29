using ProjetCESI.Core;
using ProjetCESI.Data.Metier;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ProjetCESI.Data.Metier.StatistiqueData;

namespace ProjetCESI.Metier
{
    public interface IStatistiqueMetier : IMetier<Statistique>
    {
        Task<IEnumerable<TopObject>> GetNombreActionsMoyenneParUtilisateurs(TimestampFilter __filter, DateTimeOffset __whereBas, DateTimeOffset __whereHaut);
        Task<IEnumerable<TopObject>> GetTopRecherche(int __nbRecherche, DateTimeOffset __whereBas, DateTimeOffset __whereHaut);
        Task<IEnumerable<TopObject>> GetTopConsultation(int __nbRecherche, DateTimeOffset __whereBas, DateTimeOffset __whereHaut);
        Task<string> GenerateCSVData(int __nbRecherche, DateTimeOffset __whereBas, DateTimeOffset __whereHaut, TimestampFilter __filter);
    }
}