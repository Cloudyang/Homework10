using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionDemo.DBExtend
{
    /// <summary>
    /// 还没验证
    /// 利用linq to sql生成的sql进行截断替换
    /// </summary>
    public static class ObjectQueryExtension
    {
        
        /// <summary>
        /// 执行T-sql 语句
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static int ExecuteNonQuery(string connectionString, string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.Parameters.AddRange(parameters);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;

                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
