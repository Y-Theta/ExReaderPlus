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
    /// <summary>
    /// 文章类
    /// </summary>
    [DataContract]
    public class Passage
    {
        public string HeadName { get; set; }
        public string Content { get; set; }
    }

    /// <summary>
    /// 文章信息和文章内容链接，管理类
    /// </summary>
    public class PassageManage
    {

    }

}
