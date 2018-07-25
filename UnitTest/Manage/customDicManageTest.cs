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
        [TestMethod]
        public void InsertAVocabularyToCustomDictionaryTest()
        {
            CustomDicManage.AddACustomDictionary("InsertTestDic");
            Assert.AreEqual(1, CustomDicManage.InsertAVocabularyToCustomDictionary("InsertTestDic",
              new ExReaderPlus.WordsManager.Vocabulary
              {
                  Word = "01Test", Translation = "单词关联词典插入测试"
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

        [TestMethod]
        public void DumpWordsFromWordBookToCustomDictionaryTest()
        {
            fileDatabaseManage.instance = new fileDatabaseManage();
            WordBook.InitDictionaries();
            fileDatabaseManage.instance.GetDictionaries();
            CustomDicManage.AddACustomDictionary("dumpWordsTest");
            CustomDicManage.DumpWordsFromWordBookToCustomDictionary("dumpWordsTest",WordBook.CET6.Wordlist);

        }

    }
}
