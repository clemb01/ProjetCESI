using ProjetCESI.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public interface IUserData : IData<User>
    {
        Task<IEnumerable<User>> GetUsers();
    }
}