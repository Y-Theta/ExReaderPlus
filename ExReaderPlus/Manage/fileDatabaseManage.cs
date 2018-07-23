using ExReaderPlus.WordsManager;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ExReaderPlus.Manage
{
    /// <summary>
    /// 文件数据库管理,主要是对文件数据库读出数据到内存数据库
    /// </summary>
    public class fileDatabaseManage
    {
        public static char[] spc = new char[] { ' ', ',', '.', '?', '!', '\'', '\"', '=', '\\', '/' };
        public static fileDatabaseManage instance;
        SQLiteConnection dbfile;
        SQLiteConnection db;
        SQLiteCommand command;
        /// <summary>
        /// 构造函数，链接到文件数据库，将其读如到内存数据库
        /// </summary>
        public fileDatabaseManage()
        {
            //文件词库数据库
            string path = Path.GetFullPath("DB/dic.db");
            dbfile = new SQLiteConnection("Data Source=" + path + ";Version=3;");
            dbfile.Open();
            //内存词库数据库
            string str = "Data Source=:memory:;Version=3;New=True;";
            db = new SQLiteConnection(str);
            db.Open();
            //数据库读入内存
            dbfile.BackupDatabase(db, "main", "main", -1, null, 0);
            //command对象
            var command = new SQLiteCommand();
            command.Connection = db;
            //关闭文件词库数据库
            dbfile.Close();
            dbfile = null;
        }
        /// <summary>
        /// 返回所查询一个词汇的对象
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public Vocabulary getAWord(string word)
        {
            string commandText = "SELECT word,phonetic,translation FROM stardict WHERE word="+word;
            command.CommandText = commandText;
            var reader = command.ExecuteReader();
            var vocabulary = new Vocabulary();
            if (reader!=null) {
                vocabulary.Word = reader.GetString(0);
                vocabulary.Phonetic = reader.GetString(1);
                vocabulary.Translation = reader.GetString(2);
            }
            else
            {
                Debug.WriteLine("2018年7月20日21:54:06----------reader is Null");
            }
            return vocabulary;
        }

    }
}
