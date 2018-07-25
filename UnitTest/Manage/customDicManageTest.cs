using ExReaderPlus.Manage;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDictionary;

namespace UnitTest.Manage
{
    /// <summary>
    /// 测试词典的新建、删除、导入词库
    /// </summary>
    [TestClass]
    public class CustomDicManageTest
    {

        /// <summary>
        /// 我不把上下文DataContext设置为参数
        /// </summary>
        [TestMethod]
        public void ChangeDictionaryNameTest()
        {          
                Assert.AreEqual(true,
                    CustomDicManage.AddACustomDictionary("testDic"));
                Assert.AreEqual(true,
                    CustomDicManage.ChangeDictionaryName("testDic", "testDicChanged"));
        }




    }
}
