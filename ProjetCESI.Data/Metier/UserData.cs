using Microsoft.EntityFrameworkCore;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public class UserData : Repository<User> , IUserData
    {
        public override async Task<User> GetById(int __coreElementId)
        {
            using (DbContext db = GetContext())
            {
                return await db.Set<User>().AsNoTracking().FirstOrDefaultAsync(c => c.Id == __coreElementId);
            }
        }

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
