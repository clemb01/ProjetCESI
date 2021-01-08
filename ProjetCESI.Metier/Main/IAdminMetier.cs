using ProjetCESI.Core;
using System.Collections.Generic;

namespace ProjetCESI.Metier.Main
{
    public interface IAdminMetier
    {
        IEnumerable<User> GetUser();
    }
}