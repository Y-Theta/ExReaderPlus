using ExReaderPlus.Manage;
using ExReaderPlus.WordsManager;
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


        /// <summary>
        /// 插入单词
        /// </summary>
        [TestMethod]
        public void InsertAVocabularyToCustomDictionaryTest()
        {
           // CustomDicManage.AddACustomDictionary("InsertTestDic");
            //Assert.AreEqual(0, CustomDicManage.InsertAVocabularyToCustomDictionary("InsertTestDic",
            //  new Vocabulary
            //  {
            //      Word = "01Test", Translation = "单词关联词典插入测试"
            //  }));

            Assert.AreEqual(1, CustomDicManage.InsertAVocabularyToCustomDictionary("InsertTestDic",
              new Vocabulary
              {
                  Word = "test",
                  Translation = "测试"
              }));
        }

        /// <summary>
        /// 测试删除单词本
        /// </summary>
        [TestMethod]
        public void DeleteDictionaryTest()
        {
            CustomDicManage.AddACustomDictionary("DeleteTestDic");
            Assert.AreEqual(1, CustomDicManage.InsertAVocabularyToCustomDictionary("DeleteTestDic",
              new ExReaderPlus.WordsManager.Vocabulary
              {
                  Word = "001Test",
                  Translation = "单词关联词典插入测试"
              }));
            Assert.AreEqual(1, CustomDicManage.InsertAVocabularyToCustomDictionary("DeleteTestDic",
              new ExReaderPlus.WordsManager.Vocabulary
              {
                  Word = "002Test",
                  Translation = "单词关联词典插入测试"
              }));
            Assert.AreEqual(1, CustomDicManage.InsertAVocabularyToCustomDictionary("DeleteTestDic",
              new ExReaderPlus.WordsManager.Vocabulary
              {
                  Word = "003Test",
                  Translation = "单词关联词典插入测试"
              }));
            Assert.AreEqual(1, CustomDicManage.DeleteDictionary("DeleteTestDic"));
        }

        /// <summary>
        /// 导入功能测试
        /// </summary>
        [TestMethod]
        public void DumpWordsFromWordBookToCustomDictionaryTest()
        {
            fileDatabaseManage.instance = new fileDatabaseManage();
            WordBook.InitDictionaries();
            //fileDatabaseManage.instance.GetDictionaries();
            CustomDicManage.AddACustomDictionary("dumpWordsTest");
            CustomDicManage.DumpWordsFromWordBookToCustomDictionary("dumpWordsTest", WordBook.CET6.Wordlist);

        }

        /// <summary>
        /// 把数据库导入单词本测试
        /// </summary>
        [TestMethod]
        public void DumpWordsFromFileDataBaseToTheDiconaryForTestTest()
        {
            Assert.AreEqual(1, CustomDicManage.DumpWordsFromFileDataBaseToTheDiconaryForTest());
        }

        /// <summary>
        /// 改变单词状态位测试
        /// </summary>
        [TestMethod]
        public void ChangeTheStateOfAWordTest()
        {
            var v = new Vocabulary
            {
                Word = "changeStateTest",
                Translation = "改变状态位测试",
                YesorNo = 0
                
            };
            CustomDicManage.AddACustomDictionary("changeStateTest");
            Assert.AreEqual(1,CustomDicManage.InsertAVocabularyToCustomDictionary("changeStateTest",v));
            using(var db=new DataContext())
            {
                db.Database.Migrate();
                var test=db.Words.First(vo => vo.Id.Equals(v.Word));
                Assert.AreEqual(0, test.YesorNo);
                db.Database.CloseConnection();
            }

            CustomDicManage.ChangeTheStateOfAWord(v.Word, 1);

            using (var db = new DataContext())
            {
                db.Database.Migrate();
                var test = db.Words.First(vo => vo.Id.Equals(v.Word));
                Assert.AreEqual(1, test.YesorNo);
                db.Database.CloseConnection();
            }
        }

        [TestMethod]
        public void GetAllDictionariesNameTest()
        {
            CustomDicManage.AddACustomDictionary("GetAllDictionaryNameTest");
            var re= CustomDicManage.GetAllCustomDictionariesName();
            Assert.AreEqual(1,re.Count());
        }
    }
}
