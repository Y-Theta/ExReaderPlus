using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UserDic;

namespace UnitTest.UserDictionary {
    /// <summary>
    /// 测试EFcore的user
    /// </summary>
    [TestClass]
    public class userTest
    {
        [TestMethod]
        public void userInsertTest()
        {
            ////using语句，定义一个范围，在范围结束时处理对象。 
            //// TODO 测试时用using会报错
            //var db = new DataContext();
            //// TODO 测试时用await会出错

            ////db.Database.MigrateAsync();
            ////var user = new user { password = "password", userName = "TestName" };
            ////db.users.Add(user);//数据库加入user;

            ////db.SaveChanges();
            ////db.Database.CloseConnection();
            ////db.Database.Migrate();
            //List <user> allUsers= new List<user>();
            //allUsers = db.users.ToList();
            //Assert.AreEqual(1, allUsers.Count());
            //Assert.AreEqual("TestName", db.users.First().userName.ToString());
            //Assert.AreEqual(0, 0);
           
        }

        [TestMethod]
        public void TestIsOK()
        {
            Assert.AreEqual(0, 0);
        }
    }
}
