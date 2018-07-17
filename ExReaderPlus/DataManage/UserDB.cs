using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public struct _UserWord
    {
        //test
    }
    /// <summary>
    /// 用户的词汇，应当有状态位，已经掌握，没有掌握
    /// </summary>
    public class UserWord
    {

    }
}
