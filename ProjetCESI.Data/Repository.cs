using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetCESI.Core;
using ProjetCESI.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public class Repository<T> where T : class, IGetPrimaryKey
    {
        private int? _userId;

        public int? UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                this._userId = value;
            }
        }

        public DbContext GetContext()
        {
            DbContext context = MainContext.Create();

            return context;
        }

        virtual async public Task<T> GetById(int __coreElementId)
        {
            using (DbContext db = GetContext())
            {
                return await db.Set<T>().FindAsync(__coreElementId);
            }
        }

        virtual async public Task<IEnumerable<T>> GetAll()
        {
            using (DbContext db = GetContext())
            {
                var liste = db.Set<T>();

                if (liste.Any())
                {
                    return await liste.ToListAsync();
                }
                else
                    return await CreationDonneesTable();
            }
        }

        virtual async public Task<IEnumerable<T>> CreationDonneesTable()
        {
            return null;
        }

        virtual public async Task<bool> InsertOrUpdate(IEnumerable<T> __lstCoreElement)
        {
            foreach (T coreElement in __lstCoreElement)
            {
                bool result = await InsertOrUpdate(coreElement);
                if (result == false)
                {
                    return false;
                }
            }

            return true;
        }
        virtual public async Task<bool> InsertOrUpdate(T __coreElement, bool __forceUsageFrontale = false)
        {
            using (DbContext db = MainContext.Create())
            {
                if (db.Entry(__coreElement).State == EntityState.Detached)
                    db.Set<T>().Attach(__coreElement);

                if (__coreElement.GetPrimaryKey() == default(int))
                    db.Entry(__coreElement).State = EntityState.Added;
                else
                    db.Entry(__coreElement).State = EntityState.Modified;

                int result = await db.SaveChangesAsync(true);

                return result != default(int) ? true : false;
            }

            return false;
        }

        virtual async public Task<bool> DeleteById(int __coreElementId)
        {
            bool result = false;

            using (DbContext db = GetContext())
            {
                T entityToDelete = await db.Set<T>().FindAsync(__coreElementId);

                if (entityToDelete != null)
                {
                    if (db.Entry(entityToDelete).State == EntityState.Detached)
                        db.Set<T>().Attach(entityToDelete);

                    db.Set<T>().Remove(entityToDelete);

                    result = true;
                }
            }

            return result;
        }

        virtual async public Task<bool> Delete(T __coreElement)
        {
            try
            {
                using (DbContext db = GetContext())
                {
                    db.Set<T>().Attach(__coreElement);

                    db.Set<T>().Remove(__coreElement);

                    await db.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {                
                return false;
            }
        }

        virtual async public Task<bool> Delete(IEnumerable<T> __lstCoreElement)
        {
            try
            {
                using (DbContext db = GetContext())
                {
                    for (int i = __lstCoreElement.Count() - 1; i >= 0; i--)
                    {
                        T element = __lstCoreElement.ElementAt(i);

                        db.Set<T>().Attach(element);

                        db.Set<T>().Remove(element);
                    }

                    await db.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
