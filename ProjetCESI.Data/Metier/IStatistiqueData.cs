using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public interface IStatistiqueData : IData<Statistique>
    {
        Task<IEnumerable<TopActions>> GetNombreActionsMoyenneParUtilisateurs(TimestampFilter __filter, DateTimeOffset __whereBas, DateTimeOffset __whereHaut);
        Task<IEnumerable<TopObject>> GetTopRecherche(int __nbRecherche, DateTimeOffset __whereBas, DateTimeOffset __whereHaut, int __offset = 0);
        Task<IEnumerable<TopObject>> GetTopConsultation(int __nbRecherche, DateTimeOffset __whereBas, DateTimeOffset __whereHaut, int __offset = 0);
    }
}