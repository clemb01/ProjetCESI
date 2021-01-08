using ProjetCESI.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Data.Metier
{
    public interface IUserData
    {
        Task<IEnumerable<User>> GetUsers();
    }
}