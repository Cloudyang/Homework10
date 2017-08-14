using System.Collections.Generic;
using Common.ADOEF.Model;

namespace Common.ADOEF.Interface
{
    public interface IDBHelper
    {
        int Add<T>(T t) where T : BaseModel;
        int Delete<T>(int id) where T : BaseModel;
        List<T> GetALL<T>() where T : BaseModel;
        T GetById<T>(int id) where T : BaseModel;
        int Update<T>(T t) where T : BaseModel;
    }
}