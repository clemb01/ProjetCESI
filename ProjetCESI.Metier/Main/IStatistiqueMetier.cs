using ProjetCESI.Core;
using ProjetCESI.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public interface IStatistiqueMetier : IMetier<Statistique>
    {
        Task<IEnumerable<TopObject>> GetNombreActionsMoyenneParUtilisateurs(TimestampFilter __filter, DateTimeOffset __whereBas, DateTimeOffset __whereHaut, bool __flagExportCsv = false);
        Task<IEnumerable<TopObject>> GetTopRecherche(int __nbRecherche, DateTimeOffset __whereBas, DateTimeOffset __whereHaut);
        Task<IEnumerable<TopObject>> GetTopConsultation(int __nbRecherche, DateTimeOffset __whereBas, DateTimeOffset __whereHaut);
        Task<IEnumerable<TopObject>> GetTopExploitee(int __nbRecherche, DateTimeOffset __whereBas, DateTimeOffset __whereHaut);
        Task<string> GenerateCSVData(int __nbRecherche, DateTimeOffset __whereBas, DateTimeOffset __whereHaut, TimestampFilter __filter);
    }
}