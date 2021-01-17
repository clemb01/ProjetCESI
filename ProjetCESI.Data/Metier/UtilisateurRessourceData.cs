using Microsoft.EntityFrameworkCore;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
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
    }
}
