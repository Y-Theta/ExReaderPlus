using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ExReaderPlus.DataManage
{
    /// <summary>
    /// 创建用户的词库，可以为空，也可以导入
    /// </summary>
    public class UserDB
    {
        
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class User
    {
        private string userName;
        private string password;

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 用户生词本管理 使用面向接口编程
    /// </summary>
    public class UserVocabularyManage
    {
        /// <summary>
        /// 创建一个用户生词的数据库
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        
    }
}
