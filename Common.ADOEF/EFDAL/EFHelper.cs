using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ADOEF.Model;
using System.Data.Entity;
using Common.ADOEF.Interface;

namespace Common.ADOEF.EFDAL
{
    public class EFHelper:IDBHelper
    {
        private DbContext _DbContext;
        public EFHelper(DbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public int Add<T>(T t) where T: BaseModel
        {
            _DbContext.Set<T>().Add(t);
            return  _DbContext.SaveChanges();
        }

        public int Delete<T>(int id) where T : BaseModel
        {
            var dbSet = _DbContext.Set<T>();
            T obj = dbSet.Find(id);
            if (obj != null)
            {
                dbSet.Remove(obj);
            }
            return _DbContext.SaveChanges();
        }

        public List<T> GetALL<T>() where T : BaseModel
        {
            throw new NotImplementedException();
        }

        public T GetById<T>(int id) where T : BaseModel
        {
            throw new NotImplementedException();
        }

        public int Update<T>(T t) where T : BaseModel
        {
            throw new NotImplementedException();
        }

    }
}
