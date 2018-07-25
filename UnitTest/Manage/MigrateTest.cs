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
    /// 测试EFcore的增删查改方法
    /// </summary>
    [TestClass]
    public class MigrateTest
    {
        [TestMethod]
        public void MigrateTestMethod()
        {
            using(var db=new DataContext())
            {
                db.Database.Migrate();
                db.Database.CloseConnection();               
            }
           
        }

        [TestMethod]
        public void createNewDictionaryAndDeleteAtLast()
        {
            using(var db=new DataContext())
            {
                db.Database.Migrate();
                var dictionary = new Dictionary();
                dictionary.Id = "testDic";
                dictionary.TotalWordsNumber = 0;
                db.Dictionaries.Add(dictionary);
                db.SaveChanges();
                db.Database.CloseConnection();
                db.Database.Migrate();
                var result = db.Dictionaries.SingleOrDefault(m =>  m.Id.Equals("testDic"));
                Assert.AreEqual(0, result.DictionaryWords);

                var deleteResult=db.Dictionaries.SingleOrDefault(m => m.Id.Equals("testDic"));

                if (deleteResult != null)
                {
                    db.Dictionaries.Remove(deleteResult);
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 添加一个词到生词本中
        /// 为了测试可重现性，单词是一个不存在的单词
        /// </summary>
        [TestMethod]
        public void InsertWordToDictionary()
        {
            using(var db=new DataContext())
            {
                db.Database.Migrate();
                var dictionary = new Dictionary();
                dictionary.Id = "Dic0";
                dictionary.TotalWordsNumber = 0;
                db.Dictionaries.Add(dictionary);
                db.SaveChanges();

                var word1 = new Word { Id = "00test", Translation = "测试" };
                var word2 = new Word { Id = "00a", Translation = "一个" };

                db.Words.Add(word1);
                db.Words.Add(word2);

                var dicWord = new DictionaryWord { Word = word1, Dictionary = dictionary };
                db.DictionaryWords.Add(dicWord);
                db.SaveChanges();
                Assert.AreEqual("一个", 
                    db.Words.SingleOrDefault(
                        m=>m.Id.Equals("00a")).Translation
                        );             
                
                
            } 
        }
      
    }
}
