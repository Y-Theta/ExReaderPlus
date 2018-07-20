using System;
using System.Collections.Generic;
using System.Text;

namespace UserDic
{
    public class passage
    {
        public int id { get; set; }
        public string url { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 保留字段
        /// </summary>
        public string remainWords { get; set; }
    }
}
