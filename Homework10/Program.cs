using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstDB;
using System.Reflection;
using System.Data.Entity;

namespace Homework10
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Homework10Context homework10Context = new Homework10Context())
            {
                var companySet = homework10Context.Company;
                #region  增 在Comanpy对象中演示
                {
                    var company1 = new Company()
                    {
                        Name = "测试增加",
                        CreateTime = DateTime.Now,
                        CreatorId = 1
                    };
                    companySet.Add(company1);
                    homework10Context.SaveChanges();

                }
                #endregion

                #region 改 在Comanpy对象中演示
                {
                    #region 修改方法1
                    {
                        var company1 = companySet.Find(1005);
                        company1.Name = "演示修改方法1";
                        homework10Context.SaveChanges();
                    }
                    #endregion
                    #region 修改方法2
                    {
                        var company1 = new Company()
                        {
                            Name = "演示修改方法2",
                            CreateTime = DateTime.Now,
                            CreatorId = 1,
                            Id = 1007
                        };
                        homework10Context.Entry(company1).State = EntityState.Modified;
                        homework10Context.SaveChanges();
                    }
                    #endregion
                }
                #endregion

                #region 删 在Comanpy对象中演示
                {
                    #region 删除方法1
                    {
                        var company1 = companySet.Find(2005);
                        if (company1 != null)
                        {
                            companySet.Remove(company1);
                            homework10Context.SaveChanges();
                        }
                    }
                    #endregion
                    #region 删除方法2 
                    {
                        var company1 = companySet.Where(c=>c.Id==1006).FirstOrDefault();
                        if (company1 != null)
                        {
                            homework10Context.Entry(company1).State = EntityState.Deleted;
                            homework10Context.SaveChanges();
                        }                       
                    }
                    #endregion
                }
                #endregion

                #region 查 在Comanpy对象中演示
                var companys = companySet.Where(c => c.Id > 1000);
                Show(companys);
                #endregion
            }

            Console.ReadLine();
        }

        static void Show<T>(IQueryable<T> entityList)
        {
            Type type = typeof(T);
            foreach (var prop in type.GetProperties())
            {
                Console.Write($"{prop.Name}\t");
            }
            foreach (var entity in entityList)
            {
                Console.WriteLine();
                foreach (var prop in type.GetProperties())
                {
                    Console.Write($"{prop.GetValue(entity)}\t");
                }
            }
        }
    }
}
