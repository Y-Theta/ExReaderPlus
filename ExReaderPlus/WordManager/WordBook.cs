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

    public enum Type
    {
        CET4 = 1,
        CET6 = 2,
        KY = 3,
        TOEFL = 4,
        IETLTS = 5,
    }

    public class Vocabulary
    {
        private string word;           //单词
        private string translation;    //单词释义
        private int classification;    //单词分类
        private int yesorNo;           //单词掌握情况
        private string stateColor;

        public string Word
        {
            get { return word; }
            set { word = value; }

        }
        public string Translation
        {
            get { return translation; }
            set { translation = value; }

        }
        public int Classification
        {
            get { return classification; }
            set { classification = value; }
        }
        public int YesorNo
        {
            get { return yesorNo; }
            set { yesorNo = value; }
        }
        public string StateColor
        {
            get { return stateColor; }
            set { stateColor = value; }
        }

    }


    public class WordBook
    {
        private static List<Vocabulary> cet4_Book = new List<Vocabulary>(FetchWordBook("cet4"));
        private static List<Vocabulary> cet6_Book = new List<Vocabulary>(FetchWordBook("cet6"));
        private static List<Vocabulary> kaoyan_Book = new List<Vocabulary>(FetchWordBook("ky"));
        private static List<Vocabulary> toefl_Book = new List<Vocabulary>(FetchWordBook("toefl"));
        private static List<Vocabulary> ielts_Book = new List<Vocabulary>(FetchWordBook("ielts"));
        private static List<Vocabulary> gre_Book = new List<Vocabulary>(FetchWordBook("gre"));
        private static List<Vocabulary> all_Book = new List<Vocabulary>(CombineBook());


        public static List<Vocabulary> CET4_Book
        {
            get { return cet4_Book; }
            set { cet4_Book = value; }
        }

        public static List<Vocabulary> CET6_Book
        {
            get { return cet6_Book; }
            set { cet6_Book = value; }
        }
        public static List<Vocabulary> Kaoyan_Book
        {
            get { return kaoyan_Book; }
            set { kaoyan_Book = value; }
        }
        public static List<Vocabulary> TOEFL_Book
        {
            get { return toefl_Book; }
            set { toefl_Book = value; }
        }
        public static List<Vocabulary> IELTS_Book
        {
            get { return ielts_Book; }
            set { ielts_Book = value; }
        }
        public static List<Vocabulary> GRE_Book
        {
            get { return gre_Book; }
            set { gre_Book = value; }
        }

        public static List<Vocabulary> All_Book
        {
            get { return all_Book; }
            set { all_Book = value; }
        }

        public static void InitWordsBook()
        {
            CET4_Book = new List<Vocabulary>();
            CET6_Book = new List<Vocabulary>();
            Kaoyan_Book = new List<Vocabulary>();
            TOEFL_Book = new List<Vocabulary>();
            IELTS_Book = new List<Vocabulary>();
            GRE_Book = new List<Vocabulary>();
            All_Book = new List<Vocabulary>();
        }

        public static List<Vocabulary> CombineBook()
        {
            List<Vocabulary> vocabularies = new List<Vocabulary>();
            vocabularies = CET4_Book.Concat(CET6_Book).Concat(Kaoyan_Book).Concat(TOEFL_Book)
                .Concat(IELTS_Book).Concat(GRE_Book).ToList();
            return vocabularies;
        }

        //将导出的单词添加类别标签
        public static List<Vocabulary> SetBooks(ObservableCollection<Vocabulary> reader_sourcelist, int type)
        {
            List<Vocabulary> This_Book = new List<Vocabulary>();
            List<Vocabulary> New_Book = new List<Vocabulary>(reader_sourcelist);
            foreach (var item in New_Book)
            {
                item.Classification = type;
                item.YesorNo = 0;
                This_Book.Add(item);
            }
            return This_Book;

        }

        //筛选出未掌握的单词
        public static ObservableCollection<Vocabulary> GetNoWordBook(ObservableCollection<Vocabulary> allWordBook)
        {
            var noWordBook = new ObservableCollection<Vocabulary>();
            foreach (var word in allWordBook)
            {
                if (word.YesorNo == 0)
                {
                    word.StateColor = "#ff0000";
                    noWordBook.Add(word);
                }
            }

            return noWordBook;
        }

        //筛选出已掌握的单词
        public static ObservableCollection<Vocabulary> GetYesWordBook(ObservableCollection<Vocabulary> allWordBook)
        {
            var yesWordBook = new ObservableCollection<Vocabulary>();
            foreach (var word in allWordBook)
            {
                if (word.YesorNo == 1)
                {
                    word.StateColor = "#00ff00";
                    yesWordBook.Add(word);
                }
            }
            return yesWordBook;
        }

        //标记单词掌握颜色
        public static List<Vocabulary> MarkColor(List<Vocabulary> v)
        {
            foreach (var item in v)
            {
                if (item.YesorNo == 1)
                {
                    item.StateColor = "#00ff00";
                }
                else if (item.YesorNo == 0)
                {
                    item.StateColor = "#ff0000";
                }
            }
            return v;
        }


        // # 向数据库存储单词本数据
       public static void StorageWordBook(List<Vocabulary> new_readerbook)
       {
            UserDataDB.instance.SaveWordBook(new_readerbook);
       }

        // # 从数据库取出单词本数据
        public static List<Vocabulary> FetchWordBook(string type)
        {
            WordManage.instance.CacheAddList(UserDataDB.instance.FetchWord());
            List<Vocabulary> DataBaseBook = WordManage.instance.VocabularyFiltCache(type);
            return DataBaseBook;
        }

        private static void PrintList(List<Vocabulary> list)
        {
            if (list != null)
            {
                foreach (var item in list)
                {
                    Debug.WriteLine(item.Word);
                }
            }
            else
            {
                Debug.WriteLine("list is empty!");
            }
        }


    }
}
