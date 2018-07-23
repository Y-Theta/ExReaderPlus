using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExReaderPlus.UnitTest.Manage.DataManage
{
    [TestClass]
    public class SQLiteToolTest
    {
     
        [TestMethod]
        public void InsertValuesTest()
        {
            Assert.AreEqual(1, 1);
            //string path =Path.GetFullPath("DB/dic.db");
            //SqLiteTool _sql = new SqLiteTool("Data Source=" + path + ";Version=3;");
            //_sql.test();
            Assert.AreEqual(0,1 );
        }
    }
}
