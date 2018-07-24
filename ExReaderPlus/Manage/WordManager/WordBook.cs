
using ExReaderPlus.DatabaseManager;
using ExReaderPlus.DataManage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace ExReaderPlus.WordsManager
{
    //汤浩工作空间
    //本空间下实现单词本界面各功能实现
    //附操作 MyWordsList.xaml.cs

    /// <summary>
    /// 单词掌握情况
    /// </summary>
    enum wordState
    {
        no=0,
        yes=1
    }
    /// <summary>
    /// 单词，包含状态位、音标、意思
    /// </summary>
    public class Vocabulary
    {
        /// <summary>
        /// 音标
        /// </summary>
        public string Phonetic
        {
            get;
            set;
        }
        /// <summary>
        /// 词汇
        /// </summary>
        public string Word
        {
            get;
            set;

        }
        /// <summary>
        /// 单词意思
        /// </summary>
        public string Translation
        {
            get;
            set;

        }
        /// <summary>
        /// 单词分类标签
        /// </summary>
        public string Tag
        {
            get;
            set;
        }
        /// <summary>
        /// 单词掌握情况
        /// </summary>
        public int YesorNo
        {
            get;
            set;
        }
        
        public string StateColor
        {
            get;
            set;
        }

    }

    /// <summary>
    /// 默认的单词书;使用前调用静态方法初始化
    /// </summary>
    public class WordBook
    {
        public static Dictionary<string, Vocabulary> GaoKao
        {
            get;
            set;
        }

        public static Dictionary<string,Vocabulary> CET4
        {
            get;
            set;
        }

        public static Dictionary<string,Vocabulary> CET6
        {
            get;
            set;
        }

        public static Dictionary<string,Vocabulary> KaoYan
        {
            get;
            set;
        }

        public static Dictionary<string, Vocabulary> TOEFL
        {
            get;
            set;
        }

        public static Dictionary<string, Vocabulary> IELTS
        {
            get;
            set;
        }

        /// <summary>
        /// 初始化词典，将数据读入到每个词典内
        /// </summary>
        public static void InitDictionaries()
        {
            GaoKao = new Dictionary<string, Vocabulary>();
            CET4 = new Dictionary<string, Vocabulary>();
            CET6 = new Dictionary<string, Vocabulary>();
            KaoYan = new Dictionary<string, Vocabulary>();
            TOEFL = new Dictionary<string, Vocabulary>();
            IELTS = new Dictionary<string, Vocabulary>();

        }

        /// <summary>
        /// 将词库添加到单词书中,并且初始化
        /// </summary>
        public static void InsertWordsToDictionary( Vocabulary vocabulary)
        {
            vocabulary.YesorNo = 0;
            if (vocabulary.Tag.Contains("gk")) GaoKao.Add(vocabulary.Word, vocabulary);
            if (vocabulary.Tag.Contains("cet4")) CET4.Add(vocabulary.Word, vocabulary);
            if (vocabulary.Tag.Contains("cet6")) CET6.Add(vocabulary.Word, vocabulary);
            if (vocabulary.Tag.Contains("ky")) KaoYan.Add(vocabulary.Word, vocabulary);
            if (vocabulary.Tag.Contains("toefl")) TOEFL.Add(vocabulary.Word, vocabulary);
            if (vocabulary.Tag.Contains("IELTS")) IELTS.Add(vocabulary.Word, vocabulary);
        }

    }

}//命名空间
