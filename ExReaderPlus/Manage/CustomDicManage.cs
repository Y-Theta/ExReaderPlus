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
        /// 用户增加一个词典 
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
        /// 通过词典名删除一个词典，包括词典中存在的单词
        /// 成功返回1
        /// 删除失败，返回0
        /// 词典不存在，返回-1
        /// </summary>
        /// <returns></returns>
        public static int DeleteDictionary(string dictionaryName)
        {
            using (var db = new DataContext())
            {
                db.Database.Migrate();
                var dic = db.Dictionaries.FirstOrDefault(
                    d => d.Id.Equals(dictionaryName)
                    );
                if (dic == null) return -1;
                try
                {
                    db.Database.BeginTransaction();
                    var wordsInDictionary = db.DictionaryWords.Where(dw => dw.DictionaryId.Equals(dictionaryName));
                    foreach(var word in wordsInDictionary)
                    {
                        db.DictionaryWords.Remove(word);
                    }
                    db.SaveChanges();
                    db.Dictionaries.Remove(dic);
                    db.SaveChanges();
                    db.Database.CommitTransaction();
                    return 1 ;
                }
                catch
                {
                    db.Database.RollbackTransaction();
                    return 0 ;
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
        /// 插入一个单词到单词本。
        /// 已经存在 返回0
        /// 插入失败 返回-1
        /// 插入成功 返回1
        /// 词典不存在 返回2
        /// </summary>
        public static int InsertAVocabularyToCustomDictionary(string customDictionaryName,Vocabulary vocabulary)
        {
            using(var db=new DataContext())
            {
                db.Database.Migrate();
                var dictionary = db.Dictionaries.Find(customDictionaryName);
                if (dictionary == null) return 2;//如果词典不存在，就返回-1
                db.Words.Add(VocabularyToWord(vocabulary));//添加一个单词
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    return -1;
                }
                //无论原来单词是否存在，都在wordDictionary关系表中建立一个条目
                var Selectedword = db.DictionaryWords
                        .Where(dw => dw.WordId.Equals(vocabulary.Word))
                        .Where(dw => dw.DictionaryId.Equals(customDictionaryName))
                        .Count();
                if (Selectedword==0)//如果词典中不存在这个条目
                {
                    DictionaryWord dictionaryWord =
                            new DictionaryWord
                            {
                                WordId = vocabulary.Word,
                                DictionaryId = customDictionaryName
                            };
                    db.DictionaryWords.Add(dictionaryWord);
                 
                        db.SaveChanges();
                        return 1;               
                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// 把单词书中的词汇导入用户词库
        /// 成功 1
        /// 失败 0
        /// </summary>
        public static int DumpWordsFromWordBookToCustomDictionary(string customDictionaryName,Dictionary<string,Vocabulary> wordBook)
        {
            using (var db = new DataContext())
            {
                ///两个查询语句耗时太多，修正方案：把数据读出来，查询是否存在
                var wordCache=db.Words.ToDictionary(w=>w.Id,w=>w.Id);
                var wordDictionaryCache = db.DictionaryWords
                    .Where(dw => dw.DictionaryId.Equals(customDictionaryName))//选出为这个词汇的单词
                    .ToDictionary(dw=>dw.WordId,dw=>dw.DictionaryId);
                foreach (var v in wordBook)
                {
                    //先查找是否在上下文存在                  
                    if (!wordCache.ContainsKey(v.Key))
                    {
                        db.Words.Add(VocabularyToWord(v.Value));  
                    }
                    if (!wordDictionaryCache.ContainsKey(v.Key))
                    {
                        db.DictionaryWords.Add(new DictionaryWord
                        {
                            WordId = v.Key,
                            DictionaryId = customDictionaryName

                        });
                    }
                }
                try
                {
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    return 0;
                }
                //InsertAVocabularyToCustomDictionary(customDictionaryName, v.Value);
            }
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

                /// <summary>
        /// 把数据导入C盘考纲词汇
        /// 成功返回 1
        /// 失败返回 0
        /// 已经导入 -1
        /// </summary>
        /// <returns></returns>
        public static int DumpWordsFromFileDataBaseToTheDiconaryForTest()
        {
            using (var db = new DataContext())
            {
                db.Database.Migrate();
                if (db.Dictionaries.Find("ky")!=null) return -1;

                string[] testBookName = new string[]{
                "gk",
                "cet4",
                "cet6",
                "ky",
                "toefl",
                "ielts"
                };
                foreach (var book in testBookName)
                {
                    db.Dictionaries.Add(
                        new Dictionary
                        {
                            Id = book,
                            TotalWordsNumber=0
                        }
                        );
                }//添加词典到dictionary表             

                fileDatabaseManage.instance = new fileDatabaseManage();
                var dic = fileDatabaseManage.instance.GetAllWords();//返回所有单词

                foreach(var w in dic)
                {
                    db.Words.Add(VocabularyToWord(w.Value));
                }//添加单词到word表

                foreach (var w in dic)
                {
                    if (w.Value.Tag.Contains("gk")) db.DictionaryWords.Add(new DictionaryWord
                    {
                        DictionaryId = "gk",
                        WordId = w.Value.Word
                    });
                    if (w.Value.Tag.Contains("cet4")) db.DictionaryWords.Add(new DictionaryWord
                    {
                        DictionaryId = "cet4",
                        WordId = w.Value.Word
                    });
                    if (w.Value.Tag.Contains("cet6")) db.DictionaryWords.Add(new DictionaryWord
                    {
                        DictionaryId = "cet6",
                        WordId = w.Value.Word
                    });
                    if (w.Value.Tag.Contains("ky")) db.DictionaryWords.Add(new DictionaryWord
                    {
                        DictionaryId = "ky",
                        WordId = w.Value.Word
                    });
                    if (w.Value.Tag.Contains("toefl")) db.DictionaryWords.Add(new DictionaryWord
                    {
                        DictionaryId = "toefl",
                        WordId = w.Value.Word
                    });
                    if (w.Value.Tag.Contains("ielts")) db.DictionaryWords.Add(new DictionaryWord
                    {
                        DictionaryId = "ielts",
                        WordId = w.Value.Word
                    });
                }
                try
                {
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    return 0;
                }
                finally
                {
                    db.Database.CloseConnection();
                }
               
            }
        }

        /// <summary>
        /// 在数据库中修改一个单词的状态位，传入想要修改成的状态位
        /// 0 表示未掌握
        /// 1 表示掌握
        /// 其他状态可以自定义。修改状态位会访问数据库，建议异步
        /// 成功返回true 失败返回false
        /// </summary>
        /// <param name="vocabulary"></param>
        /// <returns></returns>
        public static bool ChangeTheStateOfAWord(string word,int state)
        {
            
            using(var db=new DataContext())
            {
                db.Database.Migrate();
                db.Words.FirstOrDefault(w=>w.Id.Equals(word)).YesorNo=state;//找到这个词语
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


    }
}
