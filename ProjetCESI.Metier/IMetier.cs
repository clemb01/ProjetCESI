using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public interface IMetier<TElement> : IMetierBase
    {
        Task<IEnumerable<TElement>> GetAll();
        Task<TElement> GetById(int __coreElementId);
        Task<bool> InsertOrUpdate(TElement __coreElement);
        Task<bool> InsertOrUpdate(IEnumerable<TElement> __lstCoreElement);
        Task<bool> Delete(TElement __coreElement);
        Task<bool> Delete(IEnumerable<TElement> __lstCoreElement);
    }
}
