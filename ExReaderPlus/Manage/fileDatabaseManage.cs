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
        
        /// <summary>
        /// 声明的文数据库静态类，用来控制数据库
        /// </summary>
        public static fileDatabaseManage instance;

        /// <summary>
        /// 内存数据库
        /// </summary>
        SQLiteConnection _dbInmemory;

        /// <summary>
        /// 文件数据库
        /// </summary>
        SQLiteConnection _dbFile;

        /// <summary>
        /// 读到的结果集
        /// </summary>
        SQLiteDataReader _reader;

        /// <summary>
        /// 数据库命令
        /// </summary>
        private SQLiteCommand _command;

        /// <summary>
        /// 获取文件数据库路径
        /// </summary>
        public String FileDataPath { get; set; }

        /// <summary>
        /// 构造函数，链接到文件数据库，将其读入到内存数据库
        /// </summary>
        public fileDatabaseManage()
        {
            var  path = "DB/dic.db";
            path = Path.GetFullPath(path);
            _dbFile = new SQLiteConnection("Data Source="+path+";Version=3;");
            _dbFile.Open();
            //内存词库数据库
            string str = "Data Source=:memory:;Version=3;New=True;";
            _dbInmemory = new SQLiteConnection(str);
            _dbInmemory.Open();
            //数据库读入内存
            _dbFile.BackupDatabase(_dbInmemory, "main", "main", -1, null, 0);
            //command对象
            this._command = new SQLiteCommand();
            this._command.Connection = _dbInmemory;
            //关闭文件词库数据库
            _dbFile.Close();
            _dbFile = null;
        }


        /// <summary>
        ///从数据库读入到WordBook
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public void GetDictionaries()
        {
            //可能出现的问题：查询string用“=”；缺少空格，缺少引号，缺少转义字符
            var commandText = "SELECT word,phonetic,translation,tag FROM stardict";

            this._command.CommandText = commandText;               
            var vocabularies = new List<Vocabulary>();
            _reader = this._command.ExecuteReader();    
            
            while (_reader.Read())
            {
                var vocabulary = new Vocabulary();
                vocabulary.Word = _reader.GetString(0);
                vocabulary.Phonetic = _reader.GetString(1);
                vocabulary.Translation = _reader.GetString(2);
                vocabulary.Tag = _reader.GetString(3);
                //可能出现问题，返回列名错误
                WordBook.InsertWordsToDictionary(vocabulary);

            }
            _reader.Close();
        }
       

    }
}
