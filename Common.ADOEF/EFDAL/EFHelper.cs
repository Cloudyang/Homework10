using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Common.ADOEF.Interface;
using System.Linq.Expressions;
using ExpressionDemo.Visitor;
using System.Data;

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

        public IQueryable Query<T>(Expression<Func<T, bool>> predicate) where T:class
        {
            return _DbContext.Set<T>().Where(predicate);
        }

        public int Delete<T>(Expression<Func<T, bool>> predicate)
        {
            ConditionBuilderVisitor visitor = new ConditionBuilderVisitor();
            visitor.Visit(predicate);
            string condition = visitor.Condition();
            string sql = $"Delete From {typeof(T).Name} where {condition}";
            return _DbContext.Database.ExecuteSqlCommand(sql);
        }

        public int Update<T>(Expression<Func<T, bool>> predicate)
        {
            return 0;
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
            _DbContext.Entry(t).State = EntityState.Modified;
            return _DbContext.SaveChanges();
        }

    }
}
