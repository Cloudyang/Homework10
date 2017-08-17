
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstDB;
using System.Reflection;
using System.Data.Entity;
using ExpressionDemo.DBExtend;
using System.Data.Objects;
using Common.ADOEF.EFDAL;

namespace Homework10
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Homework10Context homework10Context = new Homework10Context())
            {

                homework10Context.Database.Log = c => Console.WriteLine(c);

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

                    var company2 = new Company()
                    {
                        Name = "测试增加22",
                        CreateTime = DateTime.Now,
                        CreatorId = 2
                    };
                }
                #endregion

                #region 改 在Comanpy对象中演示
                {
                    #region 修改方法1
                    {
                        var company1 = companySet.Find(1005);
                        if (company1 != null)
                        {
                            company1.Name = "演示修改方法1";
                        }
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
                            Id = 3
                        };
                        homework10Context.Set<Company>().Attach(company1);
                        homework10Context.Entry(company1).State = EntityState.Modified;
                        homework10Context.SaveChanges();
                    }
                    #endregion
                    #region 修改方法3 
                    {
                        var company1 = companySet.Where(c => c.Id == 2021).FirstOrDefault();
                        if (company1 != null)
                        {
                            company1.Name = "测试修改2023";
                            // homework10Context.Set<Company>().Attach(company1); 对DbSet内部数据集修改无需Attach方法
                            homework10Context.SaveChanges();
                        }
                    }
                    #endregion
                }
                #endregion

                #region 删 在Comanpy对象中演示
                {
                    #region 删除方法1
                    {
                        var company1 = companySet.Find(2023);
                        if (company1 != null)
                        {
                            companySet.Remove(company1);
                            homework10Context.SaveChanges();
                        }
                    }
                    #endregion
                    #region 删除方法2 
                    {
                        var company1 = companySet.Where(c => c.Id == 2024).FirstOrDefault();
                        if (company1 != null)
                        {
                            homework10Context.Entry(company1).State = EntityState.Deleted;
                            homework10Context.SaveChanges();
                        }
                    }
                    #endregion

                    #region 删除方法3 
                    {
                        var company1 = companySet.Where(c => c.Id == 2025).FirstOrDefault();
                        if (company1 != null)
                        {
                            homework10Context.Set<Company>().Attach(company1);
                            homework10Context.Set<Company>().Remove(company1);
                        }
                        homework10Context.SaveChanges();
                    }
                    #endregion


                }
                #endregion

                #region 查 在Comanpy对象中演示
                //  var companys = companySet.Where(c => c.Id > 1000);
                Show(companySet);
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
