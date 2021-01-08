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
        public IEnumerable<User> GetUser()
        {
            using (DbContext ctx = GetContext())
            {
                var result = ctx.Set<User>();

                return result.ToList();
            }
        }
    }
}
