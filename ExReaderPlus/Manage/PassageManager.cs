using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ExReaderPlus.WordsManager;

namespace ExReaderPlus.Manage.PassageManager
{
    [DataContract]
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



    public class PassageManage
    {
        private static List<Passage> historyPassages = new List<Passage>();
      
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
