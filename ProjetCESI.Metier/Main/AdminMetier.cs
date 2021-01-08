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
        public IEnumerable<User> GetUser() => DataClass.GetUser();
    }

}
