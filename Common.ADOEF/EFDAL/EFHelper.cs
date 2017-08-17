using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Common.ADOEF.Interface;

namespace Common.ADOEF.EFDAL
{
    public class EFHelper
    {
        private static DbContext _DbContext;
        private static readonly object _lock = new object();
        public EFHelper(DbContext dbContext)
        {
            if (_DbContext == null)
            {
                lock (_lock)
                {
                    if (_DbContext == null)
                    {
                        _DbContext = dbContext;
                    }
                }
            }
        }

        public int Add<T>(T t) where T: class
        {
            _DbContext.Set<T>().Add(t);
            return  _DbContext.SaveChanges();
        }

        public int Delete<T>(int id) where T : class
        {
            var dbSet = _DbContext.Set<T>();
            T obj = dbSet.Find(id);
            if (obj != null)
            {
                dbSet.Remove(obj);
            }
            return _DbContext.SaveChanges();
        }

        public List<T> GetALL<T>() where T : class
        {
            return _DbContext.Set<T>().ToList();
        }

        public T GetById<T>(int id) where T : class
        {
            return _DbContext.Set<T>().Find(id);
        }

        public int Update<T>(T t) where T : class
        {
            _DbContext.Set<T>().Attach(t);
            return _DbContext.SaveChanges();
        }

    }
}
