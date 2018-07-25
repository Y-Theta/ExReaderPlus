using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ExReaderPlus.Manage.PassageManager;

using ExReaderPlus.WordsManager;

namespace ExReaderPlus.Manage.ReaderManager
{
    [DataContract]
    public class ReaderManage
    {
        private Passage readerPassage;
        private ObservableCollection<Vocabulary> readerWordLists;
        private int readerChooseMode;

        [DataMember]
        public Passage ReaderPassage
        {
            get
            { return readerPassage; }
            set
            { readerPassage = value; }
        }

        [DataMember]
        public int ReaderChooseMode
        {
            get
            { return readerChooseMode; }
            set
            { readerChooseMode = value; }
        }

        [DataMember]
        public ObservableCollection<Vocabulary> ReaderWordLists
        {
            get
            { return readerWordLists; }
            set
            { readerWordLists = value; }
        }
    }

    //缓存Reader信息
    public static class CacheReaderManage
    {
        private static ReaderManage cacheReader;
        public static ReaderManage CacheReader
        {
            get
            { return cacheReader; }
            set
            { cacheReader = value; }
        }

    }
}
