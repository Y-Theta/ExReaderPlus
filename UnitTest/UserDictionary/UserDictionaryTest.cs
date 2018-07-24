using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDictionary;

namespace UnitTest.UserDictionary
{
    [TestClass]
    public class UserDictionaryTest
    {
        /// <summary>
        /// EFcore测试
        /// </summary>
        [TestMethod]
        public void TestPassage()
        {
            using (var db = new DataContext())
            {
                db.Database.Migrate();
                db.Database.CloseConnection();
            }
        }
    }
}
