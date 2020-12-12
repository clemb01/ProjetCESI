using ProjetCESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public class MetierBase<T, TData>
        where T : class
        where TData : Repository<T>, new()
    {
        protected TData DataClass = new TData();
    }
}
