using Microsoft.EntityFrameworkCore;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data.Metier
{
    public class UserData : Repository<User> , IUserData
    {
        public async Task<IEnumerable<User>> GetUsers()
        {
            using (DbContext ctx = GetContext())
            {
                var result = ctx.Set<User>().Where(c => !c.UserName.Contains("UtilisateurSupprimé") && !c.UserName.Contains("Admin") && !c.UserName.Contains("SuperAdmin"));

                return await result.ToListAsync();
            }
        }

    }
}
