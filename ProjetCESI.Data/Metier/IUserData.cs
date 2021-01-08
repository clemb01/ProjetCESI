using ProjetCESI.Core;
using System.Collections.Generic;

namespace ProjetCESI.Data.Metier
{
    public interface IUserData
    {
        IEnumerable<User> GetUser();
    }
}