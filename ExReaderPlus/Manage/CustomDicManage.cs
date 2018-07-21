using ExReaderPlus.WordsManager;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExReaderPlus.Manage
{
    /// <summary>
    /// 自定义词库管理
    /// </summary>
    public class CustomDicManage
    {
        /// <summary>
        /// 文件数据库
        /// </summary>
        SQLiteConnection dbfile;
        /// <summary>
        /// 内存数据库
        /// </summary>
        SQLiteConnection db;
        /// <summary>
        /// 读出文件字典到内存数据库db
        /// </summary>
        public void readFileDataBase()
        {
  
        }

        //public List<Vocabulary> selectDicType(string type)
        //{

            
        //}
        /// <summary>
        /// 将软件安装包中的词库导入C盘程序应用的目录
        /// </summary>
        /// <returns></returns>
        public bool backUpfileDataBase()
        {
            /// <summary>
            /// 分隔符
            /// </summary>
            char[] spc = new char[] { ' ', ',', '.', '?', '!', '\'', '\"', '=', '\\', '/' };
            SQLiteConnection dbfile;//存放在DB下的文件
            SQLiteConnection db;
            string path = Path.GetFullPath("D");
            return true;
        }
    }
}
