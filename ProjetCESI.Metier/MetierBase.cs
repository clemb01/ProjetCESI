using ProjetCESI.Core;
using ProjetCESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public class MetierBase<T, TData>
        where T : class, IGetPrimaryKey
        where TData : Repository<T>, new()
    {
        private int? _userId;

        public int? UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                DataClass.UserId = value;
            }
        }

        private MetierFactory _metierFactory;
        public MetierFactory MetierFactory
        {
            get
            {
                if (_metierFactory == null)
                    _metierFactory = new MetierFactory();

                return _metierFactory;
            }
        }

        protected TData DataClass = new TData();

        public virtual async Task<IEnumerable<T>> GetAll() => await DataClass.GetAll();
        public virtual async Task<T> GetById(int __coreElementId) => await DataClass.GetById(__coreElementId);
        public virtual async Task<bool> InsertOrUpdate(T __coreElement) => await DataClass.InsertOrUpdate(__coreElement);
        public virtual async Task<bool> InsertOrUpdate(IEnumerable<T> __lstCoreElements) => await DataClass.InsertOrUpdate(__lstCoreElements);
        public virtual async Task<bool> DeleteById(int __coreElementId, bool __flagOnlyVirtualy) => await DataClass.DeleteById(__coreElementId);
        public virtual async Task<bool> Delete(T __coreElement) => await DataClass.Delete(__coreElement);
        public virtual async Task<bool> Delete(T __coreElement, bool __flagOnlyVirtualy) => await DataClass.Delete(__coreElement);
        public virtual async Task<bool> Delete(IEnumerable<T> __lstCoreElements) => await DataClass.Delete(__lstCoreElements);
    }
}
