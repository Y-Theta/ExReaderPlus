using ExReaderPlus.Baidu;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Baidu
{
    [TestClass]
    public class TranslateTest
    {

        [TestMethod]
        public void getResult()
        {

        }

        [TestMethod]
        public void GetJsonTest()
        {
            var t = new Translate();
            t.Text = "test";
            var s = t.GetResult();
            Assert.AreEqual("测试", s);
        }

        
    }
}
