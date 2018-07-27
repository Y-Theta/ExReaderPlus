using ExReaderPlus.Manage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExReaderPlus.WordsManager;

namespace UnitTest.Manage
{
    [TestClass]
    public class fileDatabaseManageTest
    {

        /// <summary>
        /// 测试打开文件数据库,将数据库读入内存数据库
        /// </summary>
        [TestMethod]
        public void openFileDatabaseTest()
        {
            fileDatabaseManage.instance = new fileDatabaseManage();//创建一个静态实例
        }

        /// <summary>
        /// 测试是否获得单词书，也展示了单词书的用法。在Map中可以静态修改状态位
        /// 此操作在程序启动时初始化
        /// </summary>
        [TestMethod]
        public void testWordBook()
        {
           
        }

        [TestMethod]
        public void SearchVocabularyTest()
        {
            fileDatabaseManage.instance = new fileDatabaseManage();
            var v1=fileDatabaseManage.instance.SearchVocabulary("test");
            Assert.AreEqual(true, v1.Translation.Contains("测试"));
            var v2 = fileDatabaseManage.instance.SearchVocabulary("00");
            Assert.AreEqual(null, v2);
        }
    }
}
