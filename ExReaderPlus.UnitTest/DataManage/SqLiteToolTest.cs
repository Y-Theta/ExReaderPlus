using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExReaderPlus.DataManage;
using System.IO;
using System.Data.SQLite;

namespace ExReaderPlus.UnitTest.DataManage
{
    [TestClass]
    public class SqLiteToolTest
    {
        private static SqLiteTool _sql;

        [TestMethod]
        public void IsertValuesTest()
        {
            _sql = new SqLiteTool("data sourse=DB/user.db");
            _sql.InsertValues("userinfo", new string[] { "9DemonFox", "password" });
            SQLiteDataReader reader = _sql.ReadFullTable("userinfo");
             Assert.AreEqual("9DemonFox",reader.GetString(0));

        }
    }
}
