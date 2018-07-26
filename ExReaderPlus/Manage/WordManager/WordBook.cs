using ExReaderPlus.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using UserDictionary;

namespace ExReaderPlus.WordsManager {

    /// <summary>
    /// 单词掌握情况
    /// </summary>
    enum wordState {
        no = 0,
        yes = 1
    }


    /// <summary>
    /// 字典
    /// </summary>
    public class EngDictionary {
        public string Name { get; set; }

        public bool IsSystem { get; set; }

        public int Dicname { get; set; }

        public Dictionary<string, Vocabulary> Wordlist { get; set; }

        public bool HasWord(string str) {
            return Wordlist.ContainsKey(str);
        }

        public EngDictionary() {
            Wordlist = new Dictionary<string, Vocabulary>();
        }

        public BriefDic GetBriefDic() {
            return new BriefDic { Name = Name, IsSys = IsSystem, DicName = Dicname, WordsCount = Wordlist.Count };
        }

    }

    /// <summary>
    /// 单词，包含状态位、音标、意思
    /// </summary>
    public class Vocabulary {
        /// <summary>
        /// 音标
        /// </summary>
        public string Phonetic {
            get;
            set;
        }

        /// <summary>
        /// 词汇
        /// </summary>
        public string Word {
            get;
            set;

        }
        /// <summary>
        /// 单词意思
        /// </summary>
        public string Translation {
            get;
            set;

        }
        /// <summary>
        /// 单词分类标签
        /// </summary>
        public string Tag {
            get;
            set;
        }
        /// <summary>
        /// 单词掌握情况
        /// </summary>
        public int YesorNo {
            get;
            set;
        }

        public string StateColor {
            get;
            set;
        }

    }

    /// <summary>
    /// 默认的单词书;使用前调用静态方法初始化
    /// </summary>
    public class WordBook {

        public static bool Initdicready = false;

        public static int SelectedDic;

        public static EngDictionary GaoKao { get; set; }

        public static EngDictionary CET4 { get; set; }

        public static EngDictionary CET6 { get; set; }

        public static EngDictionary KaoYan { get; set; }

        public static EngDictionary TOEFL { get; set; }

        public static EngDictionary IELTS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static EngDictionary GetDic(int i) {
            switch (i)
            {
                case 0: return GaoKao;
                case 1: return CET4;
                case 2: return CET6;
                case 3: return KaoYan;
                case 4: return TOEFL;
                case 5: return IELTS;
                default: return null;
            }
        }

        public static EngDictionary GetDicNow() {
            return GetDic(SelectedDic);
        }

        /// <summary>
        /// 初始化词典，将数据读入到每个词典内
        /// </summary>
        public static void InitDictionaries() {
            GaoKao = new EngDictionary { Name = "高考词汇", IsSystem = true,Dicname = 0 };

            CET4 = new EngDictionary { Name = "大学四级", IsSystem = true, Dicname = 1};

            CET6 = new EngDictionary { Name = "大学六级", IsSystem = true, Dicname = 2};

            KaoYan = new EngDictionary { Name = "考研词汇", IsSystem = true, Dicname = 3 };

            TOEFL = new EngDictionary { Name = "托福词汇", IsSystem = true, Dicname = 4};

            IELTS = new EngDictionary { Name = "雅思词汇", IsSystem = true, Dicname = 5 };

        }

        /// <summary>
        /// 将词库添加到单词书中,并且初始化
        /// </summary>
        public static void InsertWordsToDictionary(Vocabulary vocabulary) {
            vocabulary.YesorNo = 0;
            if (vocabulary.Tag.Contains("gk")) GaoKao.Wordlist.Add(vocabulary.Word, vocabulary);
            if (vocabulary.Tag.Contains("cet4")) CET4.Wordlist.Add(vocabulary.Word, vocabulary);
            if (vocabulary.Tag.Contains("cet6")) CET6.Wordlist.Add(vocabulary.Word, vocabulary);
            if (vocabulary.Tag.Contains("ky")) KaoYan.Wordlist.Add(vocabulary.Word, vocabulary);
            if (vocabulary.Tag.Contains("toefl")) TOEFL.Wordlist.Add(vocabulary.Word, vocabulary);
            if (vocabulary.Tag.Contains("IELTS")) IELTS.Wordlist.Add(vocabulary.Word, vocabulary);
        }

        /// <summary>
        /// 返回一个词库 
        /// 默认词典参数 gk cet4 cet6 ky ielts toefl
        /// </summary>
        /// <param name="dictionaryName"></param>
        public static EngDictionary GetDictionaryByName(string dictionaryName)
        {

            using(var db=new DataContext())
            {
                db.Database.Migrate();
                var result = db.DictionaryWords
                    .Include(m => m.Word)
                    .Include(m => m.Dictionary)
                    .Where(m => m.Dictionary.Id == dictionaryName);

                var dic = new EngDictionary
                {

                };
                foreach (var r in result)
                {
                    var v = new Vocabulary();
                    v.Word = r.Word.Id;
                    v.YesorNo= r.Word.YesorNo;
                    v.Translation = r.Word.Translation;
                    v.Phonetic = r.Word.Translation;
                    dic.Wordlist.Add(v.Word, v);
                }
                db.Database.CloseConnection();
                return dic;

            }
            
        }
    }

}//命名空间
