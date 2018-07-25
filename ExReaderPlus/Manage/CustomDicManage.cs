using ExReaderPlus.WordsManager;
using Microsoft.EntityFrameworkCore;
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
    /// 用户自定义词典管理，包括新建词典、删除词典、
    /// 读出词典所有单词、实时保存单词、修改单词状态位
    /// </summary>
    public static class CustomDicManage
    {
        
        /// <summary>
        /// TODO 修改为异步的方法 用户增加一个词典
        /// 如果词典名已存在，则返回FALSE
        /// </summary>
        public static bool AddACustomDictionary(string DictionaryName)
        {
            using(var db=new DataContext())
            {
                db.Database.Migrate();
                if (db.Dictionaries.Find(DictionaryName) == null)
                {
                    var dictionary = new Dictionary();
                    dictionary.Id = DictionaryName;
                    dictionary.TotalWordsNumber = 0;
                    db.Dictionaries.Add(dictionary);
                    db.SaveChanges();
                    db.Database.CloseConnection();
                    return true;
                }
                else
                {
                    db.Database.CloseConnection();
                    return false;                   
                }
            }
        }
        
        /// <summary>
        /// 修改词典名字,若先后名字相同，返回错误
        /// 由于是选中词典，所以不考虑词典不存在
        /// </summary>
        /// <param name="originalName"></param>
        /// <param name="currentName"></param>
        /// <returns></returns>
        public static bool ChangeDictionaryName(string originalName,string currentName)
        {
            if (originalName == currentName)
                return false;
            else
            {
                using (var db = new DataContext())
                {
                    db.Database.Migrate();
                    var dic = db.Dictionaries.FirstOrDefault(
                        d=>d.Id.Equals(originalName)
                        );
                    db.Dictionaries.Remove(dic);
                    db.SaveChanges();
                    dic.Id = currentName;
                    db.Dictionaries.Add(dic);
                    db.SaveChanges();
                    db.Database.CloseConnection();
                }
                return true;
            }
        }

        /// <summary>
        /// 通过词典名删除一个词典
        /// 如果删除失败，所有事物回滚
        /// </summary>
        /// <returns></returns>
        public static bool DeleteDictionary(string dictionaryName)
        {
            using (var db = new DataContext())
            {
                db.Database.Migrate();
                var dic = db.Dictionaries.FirstOrDefault(
                    d => d.Id.Equals(dictionaryName)
                    );
                db.Dictionaries.Remove(dic);
                try
                {
                    db.Database.BeginTransaction();
                    db.SaveChanges();
                    var wordsInDictionary = db.DictionaryWords.Where(dw => dw.DictionaryId.Equals(dictionaryName));
                    foreach(var word in wordsInDictionary)
                    {
                        db.DictionaryWords.Remove(word);
                    }
                    db.SaveChanges();
                    db.Database.CommitTransaction();
                    return true ;
                }
                catch
                {
                    db.Database.RollbackTransaction();
                    return false;
                }
                finally
                {
                    db.Database.CloseConnection();
                }

            }
        }
        /// <summary>
        /// 把Vocabulary转化为Word
        /// </summary>
        /// <returns></returns>
        public static Word VocabularyToWord(Vocabulary vocabulary)
        {
            var word = new Word();
            word.Id = vocabulary.Word;
            word.Tag = vocabulary.Tag;
            word.Translation = vocabulary.Translation;
            word.YesorNo = vocabulary.YesorNo;
            word.Phonetic = vocabulary.Phonetic;
            return word;
        }
        /// <summary>
        /// 插入单词到单词本。如果插入失败，那么返回FALSE
        /// 原来已经存在，则返回0
        /// 插入失败 返回-1
        /// 插入成功 返回1
        /// </summary>
        public static int InsertAVocabularyToCustomDictionary(string dictionaryName,Vocabulary vocabulary)
        {
            using(var db=new DataContext())
            {
                db.Database.Migrate();
                var dictionary = db.Dictionaries.FirstOrDefault(v => v.Id.Equals(dictionaryName));
                db.Words.Add(VocabularyToWord(vocabulary));//添加一个单词
                try
                {
                    db.SaveChanges();
                }
                catch
                {

                }
                //无论原来单词是否存在，都在wordDictionary关系表中建立一个条目
                var Selectedword = db.DictionaryWords
                        .Where(dw => dw.WordId.Equals(vocabulary.Word))
                        .Where(dw => dw.DictionaryId.Equals(dictionaryName))
                        .FirstOrDefault();
                if (Selectedword != null)//如果词典中不存在这个条目
                {
                    DictionaryWord dictionaryWord =
                            new DictionaryWord
                            {
                                WordId = vocabulary.Word,
                                DictionaryId = dictionaryName
                            };
                    db.DictionaryWords.Add(dictionaryWord);
                    try
                    {
                        db.SaveChanges();
                        return 1;
                    }
                    catch
                    {
                        return -1;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 把单词书中的词汇导入用户词库
        /// </summary>
        public static void DumpWordsFromWordBookToCustomDictionary()
        {

        }

        /// <summary>
        /// 将用户自定义词典读出到Dictionary<string,Vocabulary>
        /// 定义了两个词汇类，暂时妥协，读出来重新赋值
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,Vocabulary> ReadCustomDictionary(string DictionaryName)
        {
            var dictionary = new Dictionary<string, Vocabulary>();
            using (var db = new DataContext())
            {
                db.Database.Migrate();
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
