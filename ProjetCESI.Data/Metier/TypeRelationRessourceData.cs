using Microsoft.EntityFrameworkCore;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public class TypeRelationRessourceData : Repository<TypeRelationRessource>, ITypeRelationRessourceData
    {
        public async Task<IEnumerable<TypeRelationRessource>> GetTypeRelationRessourcesByRessourceId(int __ressourceId)
        {
            using(var ctx = GetContext())
            {
                return await ctx.Set<TypeRelationRessource>()
                            .Include(c => c.TypeRelation)
                            .Where(c => c.RessourceId == __ressourceId)
                            .ToListAsync();
            }
        }
    }
}
