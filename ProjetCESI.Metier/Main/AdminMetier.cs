using ProjetCESI.Core;
using ProjetCESI.Data.Metier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier.Main
{
    public class AdminMetier : MetierBase<User, UserData>, IAdminMetier
    {

        public async Task<IEnumerable<User>> GetUser() => await DataClass.GetUsers();

        public async Task<bool> AnonymiseUser(User user)
        {
            if(user != null)
            {
                user.UserName = $"UtilisateurSupprimé_{DateTime.Now.Ticks}";
                user.Email = "";

                var result = await DataClass.InsertOrUpdate(user);
                return result;
            }
            return false;
        }

        public async Task<bool> BanUserTemporary(User user)
        {
            if(user != null)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.Now.AddMinutes(3);

                var result = await DataClass.InsertOrUpdate(user);
                return result;
            }
            return false;
        }

        public async Task<bool> DeBan(User user)
        {
            if(user != null)
            {
                user.LockoutEnd = null;
                var result = await DataClass.InsertOrUpdate(user);
                return result;
            }
            return false;
        }
    }



}
