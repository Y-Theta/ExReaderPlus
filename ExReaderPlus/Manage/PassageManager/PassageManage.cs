using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ExReaderPlus.DatabaseManager;
using ExReaderPlus.WordsManager;

namespace ExReaderPlus.Manage.PassageManager
{

    public class Passage
    {
        private string headName;  //文章标题
        private string content;   //文章内容
        private List<Tuple<int, int>> highLightInfo = new List<Tuple<int, int>>();//文章高亮信息

        [DataMember]
        public string HeadName { get { return headName; } set { headName = value; } }
        [DataMember]
        public string Content { get { return content; } set { content = value; } }     //存储文本内容
        [DataMember]
        public List<Tuple<int, int>> HighLightInfo
        {
            get { return highLightInfo; }
            set { highLightInfo = value; }
        }

    }
    class PassageManage
    {
        private static List<Passage> historyPassages = new List<Passage>();
        public static List<Passage> HistoryPassages
        {
            get
            { return LoadPassages(); }
            set
            { historyPassages = value; }
        }

        //保存文章到数据库
        public static void SavePassage(Passage passage)
        {
            List<Passage> database_passages = new List<Passage>();
            database_passages = UserDataDB.instance.LoadPassage();

            foreach (var i in database_passages)
            {
                Debug.WriteLine(i.Content + "\n");
            }
            historyPassages = database_passages;
            int index = historyPassages.IndexOf(historyPassages.Where(x => x.HeadName == passage.HeadName).FirstOrDefault());
            if (index < 0)
            {

                historyPassages.Add(passage);
                UserDataDB.instance.SavaPassage(passage); //文章存入数据库
            }
            else
            {
                historyPassages.RemoveAt(index);
                historyPassages.Add(passage);
                UserDataDB.instance.SavaPassage(passage); //文章存入数据库
            }

        }

        //加载历史文章
        public static List<Passage> LoadPassages()
        {
            return UserDataDB.instance.LoadPassage();
        }

        //清空历史文章
        public static void ClearPassages()
        {
            List<Passage> passages = UserDataDB.instance.LoadPassage();
            foreach (var p in passages)
            {
                UserDataDB.instance.DeletePassage(p.HeadName);
            }
        }

        //删除文章
        public static void DeletePassage(Passage p)
        {
            try { UserDataDB.instance.DeletePassage(p.HeadName); }
            catch { }
        }


        //获取初始文章
        public Passage GetPassageAsync()
        {


            Passage p = new Passage();


            p.Content = "foreign";
            p.HeadName = "Example";
            return p;
        }


        //获取单词高亮位置
        public static List<Tuple<int, int>> GetHighLightInfo(String content, ObservableCollection<Vocabulary> readerlist)
        {
            List<Tuple<int, int>> WordInfo = new List<Tuple<int, int>>();
            foreach (var a in readerlist)
            {
                int index = content.IndexOf(a.Word);
                Tuple<int, int> tuple = new Tuple<int, int>(index, a.Word.Length);
                WordInfo.Add(tuple);

            }
            return WordInfo;
        }

    }
}
