using ExReaderPlus.Manage;
using ExReaderPlus.Models;
using ExReaderPlus.View;
using ExReaderPlus.View.Commands;
using ExReaderPlus.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public int LearneedWords { get; set; }

        public Dictionary<string, Vocabulary> Wordlist { get; set; }

        public bool HasWord(string str) {
            return Wordlist.ContainsKey(str);
        }

        public EngDictionary() {
            Wordlist = new Dictionary<string, Vocabulary>();
        }

        public ActionDictionary GetActionDictionary() {
            return new ActionDictionary { Name = Name, IsSys = IsSystem, DBName = Dicname, WordsCount = Wordlist.Count, LearnedWords = LearneedWords };
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


    }

    /// <summary>
    /// 默认的单词书;使用前调用静态方法初始化
    /// </summary>
    public class WordBook {

        public static int CustomeDicCounts = 0;

        public static bool Initdicready = false;

        public static event EventHandler SelectedDicChanged;
        private static int _selectedDic;
        public static int SelectedDic {
            get => _selectedDic;
            set { _selectedDic = value; SelectedDicChanged?.Invoke(null, EventArgs.Empty); }
        }

        public static EngDictionary GaoKao { get; set; }

        public static EngDictionary CET4 { get; set; }

        public static EngDictionary CET6 { get; set; }

        public static EngDictionary KaoYan { get; set; }

        public static EngDictionary TOEFL { get; set; }

        public static EngDictionary IELTS { get; set; }


        public static List<EngDictionary> Custom = new List<EngDictionary>();

        /// <summary>
        /// 
        /// </summary>
        public static EngDictionary GetDic(int i) {
            if (i < 10)
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
            else
                return Custom[i - 10];
        }

        public static List<ActionDictionary> GetCustomeDics() {
            List<ActionDictionary> avl = new List<ActionDictionary>();
            if (Custom.Count != 0)
            {
                foreach (var re in Custom)
                {
                    avl.Add(re.GetActionDictionary());
                    CustomeDicCounts++;
                }
                return avl;
            }
            else
                return null;
        }

        public static EngDictionary GetDicNow() {
            return GetDic(SelectedDic);
        }

        /// <summary>
        /// 初始化词典，将数据读入到每个词典内
        /// </summary>
        public static void InitDictionaries() {
            GaoKao = new EngDictionary { Name = "高考词汇", IsSystem = true, Dicname = 0 };
            CET4 = new EngDictionary { Name = "大学四级", IsSystem = true, Dicname = 1 };
            CET6 = new EngDictionary { Name = "大学六级", IsSystem = true, Dicname = 2 };
            KaoYan = new EngDictionary { Name = "考研词汇", IsSystem = true, Dicname = 3 };
            TOEFL = new EngDictionary { Name = "托福词汇", IsSystem = true, Dicname = 4 };
            IELTS = new EngDictionary { Name = "雅思词汇", IsSystem = true, Dicname = 5 };

            var customDicNameList = CustomDicManage.GetAllCustomDictionariesName();

            if (customDicNameList.Count!=0)
            {
                foreach (var re in customDicNameList)
                {
                    var i = customDicNameList.IndexOf(re);
                    Custom.Add(new EngDictionary { Name = re, IsSystem = false, Dicname = i + 10 });//用户词典的DicName=i+10
                }
            }
        }

        public static async Task InitDicCollectionAsync() {
            Task tk = new Task(() => {
                int k = 0;
                GaoKao.Wordlist = GetDictionaryByName("gk",out k);
                GaoKao.LearneedWords = k;
                CET4.Wordlist = GetDictionaryByName("cet4", out k);
                CET4.LearneedWords = k;
                CET6.Wordlist = GetDictionaryByName("cet6", out k);
                CET6.LearneedWords = k;
                KaoYan.Wordlist = GetDictionaryByName("ky", out k);
                KaoYan.LearneedWords = k;
                TOEFL.Wordlist = GetDictionaryByName("toefl", out k);
                TOEFL.LearneedWords = k;
                IELTS.Wordlist = GetDictionaryByName("ielts", out k);
                IELTS.LearneedWords = k;
                foreach (var dic in Custom)
                {
                    dic.Wordlist = GetDictionaryByName(dic.Name,out k);
                    dic.LearneedWords = k;
                }
                Initdicready = true;
            });
            tk.Start();
            await tk;
        }


        /// <summary>
        /// 返回一个词库 
        /// 默认词典参数 gk cet4 cet6 ky ielts toefl
        /// </summary>
        /// <param name="dictionaryName"></param>
        public static Dictionary<string, Vocabulary> GetDictionaryByName(string dictionaryName,out int learned)
        {
            Dictionary<string, Vocabulary> keyValues = new Dictionary<string, Vocabulary>();
            learned = 0;
            using (var db = new DataContext())
            {
                db.Database.Migrate();
                var result = db.DictionaryWords
                    .Include(m => m.Word)
                    .Include(m => m.Dictionary)
                    .Where(m => m.Dictionary.Id == dictionaryName);

                foreach (var r in result)
                {
                    var v = new Vocabulary();
                    v.Word = r.Word.Id;
                    v.YesorNo = r.Word.YesorNo;
                    v.Translation = r.Word.Translation;
                    v.Phonetic = r.Word.Phonetic;
                    if (v.YesorNo == 1)
                        learned++;
                    keyValues.Add(v.Word, v);
                }

                db.Database.CloseConnection();
                return keyValues;
            }
        }

        public static async Task<bool> ChangeWordStatePenetrateAsync(string str, int state) {
            GetDicNow().Wordlist[str].YesorNo = state;
            if (state == 1)
                GetDicNow().LearneedWords += 1;
            else
                GetDicNow().LearneedWords -= 1;
            (App.Current.Resources["DicPageViewModel"] as DicPageViewModel).UpdateDicinfo();
            return await CustomDicManage.ChangeTheStateOfAWord(str, state);
        }
    }

}//命名空间
