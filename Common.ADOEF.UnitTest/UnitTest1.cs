using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.ADOEF.EFDAL;
using System.Data.Entity;
using CodeFirstDB;

namespace Common.ADOEF.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private EFHelper _EFHelper;

        [TestMethod]
        public void TestMethod1()
        {
        }


        public UnitTest1()
        {
            var _DbContext =  new Homework10Context();
            _EFHelper = new EFHelper(_DbContext);
        }

        [TestMethod]
        public void TestAdd()
        {
            var company2 = new Company()
            {
                Name = "测试增加22",
                CreateTime = DateTime.Now,
                CreatorId = 2
            };
            var iResult = _EFHelper.Add(company2);
            
            Assert.AreEqual(iResult, 1);
        }
    }
}
