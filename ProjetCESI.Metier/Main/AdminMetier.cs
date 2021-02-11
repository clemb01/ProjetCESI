using ProjetCESI.Core;
using ProjetCESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public class AdminMetier : MetierBase<User, UserData>, IAdminMetier
    {

        public async Task<IEnumerable<User>> GetUser() => await DataClass.GetUsers();

        public async Task<bool> AnonymiseUser(User user)
        {
            if(user != null)
            {
                user.UserName = $"UtilisateurSupprimé_{DateTime.Now.Ticks}";
                user.NormalizedUserName = user.UserName.ToUpper();
                user.Email = "";
                user.NormalizedEmail = "";
                user.UtilisateurSupprime = true;

                var result = await DataClass.InsertOrUpdate(user);
                return result;
            }
            return false;
        }

        public async Task<bool> BanUserTemporary(User user, int time)
        {
            if(user != null)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.Now.AddDays(time);

                var result = await DataClass.InsertOrUpdate(user);
                return result;
            }
            return false;
        }

        public async Task<bool> BanUserPermanent(User user)
        {
            if(user != null)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.MaxValue;

                var resultban = await DataClass.InsertOrUpdate(user);
                return resultban;
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

        public async Task<User> UpdateInfoUser(User user, string newUsername)
        {
            if(user != null)
            {
                user.UserName = newUsername;
                user.NormalizedUserName = newUsername.ToUpper();
                var result = await DataClass.InsertOrUpdate(user);
                return user;
            }
            return null;
        }

    }



}
