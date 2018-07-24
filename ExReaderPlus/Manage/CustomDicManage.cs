using ExReaderPlus.WordsManager;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDictionary;

namespace ExReaderPlus.Manage
{
    /// <summary>
    /// 用户自定义词典管理
    /// </summary>
    public class CustomDicManage
    {
        
        /// <summary>
        /// TODO 修改为异步的方法 用户增加一个词典
        /// 如果词典名已存在，则返回FALSE
        /// </summary>
        public bool AddACustomDictionary(string DictionaryName)
        {
            using(var db=new DataContext())
            {
                if (db.Dictionaries.Find(DictionaryName) == null)
                {
                    var dictionary = new Dictionary();
                    dictionary.Id = DictionaryName;
                    dictionary.TotalWordsNumber = 0;
                    db.Dictionaries.Add(dictionary);
                    db.SaveChanges();
                    return true;
                }
                else return false;
            }
        }
        


        /// <summary>
        /// 将用户自定义词典读出到Dictionary<string,Vocabulary>
        /// 定义了两个词汇类，暂时妥协，读出来重新赋值
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,Vocabulary> ReadCustomDictionary(string DictionaryName)
        {
            var dictionary = new Dictionary<string, Vocabulary>();
            using (var db = new DataContext())
            {
                var listOfWords=db.Words.ToList();
                foreach(Word word in db.Words)
                {
                    var vocabulary = new Vocabulary();
                    vocabulary.Word = word.Id;
                    vocabulary.YesorNo = word.YesorNo;
                    vocabulary.Translation = word.Translation;
                    vocabulary.Tag = word.Tag;
                    vocabulary.Phonetic = word.Phonetic;
                    dictionary.Add(vocabulary.Word,vocabulary);
                }
            }
                return dictionary;
        }
    }
}
