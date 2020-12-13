using Microsoft.EntityFrameworkCore;
using ProjetCESI.Core;
using ProjetCESI.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data.Metier
{
    public class ApplicationRoleData : Repository<ApplicationRole>, IApplicationRoleData
    {
        public override async Task<IEnumerable<ApplicationRole>> CreationDonneesTable()
        {
            List<ApplicationRole> listeRoles = new List<ApplicationRole>();

            foreach (var enumeration in Enum.GetNames(typeof(TypeUtilisateur)))
            {
                listeRoles.Add(new ApplicationRole(enumeration) { NormalizedName = enumeration.ToUpper() });
            }

            await InsertOrUpdate(listeRoles);

            return listeRoles;
        }
    }
}
