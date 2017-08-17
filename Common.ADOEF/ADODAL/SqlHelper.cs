using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Interface.BaseModel;

namespace Common.ADOEF.ADODAL
{
    public class SqlHelper : Interface.IDBHelper
    {
        //更新代码：做成静态，免得每次都要去读配置文件
        private static string sConn = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        /// <summary>
        /// 执行数据库操作
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="sqlParameters">参数化</param>
        /// <returns></returns>
        private static int exec(string sql, SqlParameter[] sqlParameters = null)
        {
            int iResult;
            using (SqlConnection conn = new SqlConnection(sConn))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                if (sqlParameters != null)
                {
                    command.Parameters.AddRange(sqlParameters);
                }
                conn.Open();
                iResult = command.ExecuteNonQuery();
            }

            return iResult;
        }

        public int Add<T>(T t) where T : BaseModel
        {

            int iResult = 0;
            Type type = typeof(T);
            string fieldList = string.Join(",", type.GetProperties()
                                                    .Where(p => !p.Name.Equals("Id"))
                                                    .Select(p => string.Format("[{0}]", p.Name)));
            //更新代码：将valueList 替换参数型 parameterList
            string parameterList = string.Join(",", type.GetProperties()
                                                    .Where(p => !p.Name.Equals("Id"))
                                                    .Select(p => string.Format("@{0}", p.Name)));
            //新增代码：带参数部分
            var pvList = type.GetProperties()
                             .Where(p => !p.Name.Equals("Id"))
                             .Select(p => new SqlParameter
                             {
                                 ParameterName = string.Format("@{0}", p.Name),
                                 Value = p.GetValue(t) ?? DBNull.Value
                             }).ToArray();

            string sql = string.Format("insert into [{0}]({1}) values({2})", type.Name, fieldList, parameterList);
            iResult = exec(sql, pvList);

            return iResult;
        }


        public int Update<T>(T t) where T : BaseModel
        {
            /*update [User]  set  [Account]='yy'   where id=1    */
            int iResult = 0;
            Type type = typeof(T);
            string fieldList = string.Join(",", type.GetProperties()
                                                    .Where(p => !p.Name.Equals("Id"))
                                                    .Select(p => string.Format("[{0}]=@{0}", p.Name)));
            //新增代码：带参数部分
            var pvList = type.GetProperties()
                             .Where(p => !p.Name.Equals("Id"))
                             .Select(p => new SqlParameter
                             {
                                 ParameterName = string.Format("@{0}", p.Name),
                                 Value = p.GetValue(t) ?? DBNull.Value
                             }).ToArray();

            string sql = string.Format("update [{0}] set {1}  where id={2}", type.Name, fieldList, t.Id);
            iResult = exec(sql, pvList);

            return iResult;
        }


        public int Delete<T>(int id) where T : BaseModel
        {
            /*delete from [User]  where id=1    */
            int iResult = 0;
            Type type = typeof(T);
            string sql = string.Format("delete from [{0}] where id={1}", typeof(T).Name, id);

            iResult = exec(sql);
            return iResult;
        }

        public T GetById<T>(int id) where T : BaseModel
        {
            /*
            select [Name],[Account],[Password],[Email],[Mobile],[CompanyId],[CompanyName],[State],[UserType],[LastLoginTime],CreateTime],[CreatorId],[LastModifierId],[LastModifyTime]
            from [User] where id={0}
            */
            Type type = typeof(T);
            string fieldList = string.Join(",", type.GetProperties().Select(s => string.Format("[{0}]", s.Name)));
            string sql = string.Format(@"select {0} from [{1}] where id={2}",
                                    fieldList, type.Name, id);
            var obj = Activator.CreateInstance<T>();
            using (SqlConnection conn = new SqlConnection(sConn))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = command.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                if (dr.Read())
                {
                    foreach (var item in type.GetProperties())
                    {
                        item.SetValue(obj, dr[item.Name]);
                    }
                }
            }

            return obj;
        }

        public List<T> GetALL<T>() where T : BaseModel
        {
            List<T> ts = new List<T>();
            Type type = typeof(T);
            string fieldList = string.Join(",", type.GetProperties().Select(p => string.Format("[{0}]", p.Name)));
            string sql = string.Format("select {0} from [{1}]", fieldList, type.Name);

            using (SqlConnection conn = new SqlConnection(sConn))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    T t = Activator.CreateInstance<T>();
                    foreach (var item in type.GetProperties())
                    {
                        //更新代码：解决System.ArgumentException:类型System.DBNull的对象无法转换为类型System.String。
                        //if (dr[item.Name] is DBNull)
                        //{
                        //    item.SetValue(t, null);
                        //}
                        //else
                        //{
                        //    item.SetValue(t, dr[item.Name]);
                        //}
                        item.SetValue(t, dr[item.Name] is DBNull ? default(T) : dr[item.Name]);
                    }
                    ts.Add(t);
                }
            }

            return ts;
        }
    }
}
