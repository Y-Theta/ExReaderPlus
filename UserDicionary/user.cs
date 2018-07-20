using System;
using System.Collections.Generic;
using System.Text;

namespace UserDic
{
    /// <summary>
    /// 用户
    /// </summary>
    public class user
    {
        public int id { get; set; }

        public string userName { get; set; }

        public string password { get; set; }

        /// <summary>
        /// 保留字段
        /// </summary>
        public string remainWords_1 { get; set; }

        public string remainWords_2 { get; set; }
    }
}
