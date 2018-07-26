using System;
using System.Collections.Generic;
using System.Text;

namespace UserDictionary
{
    public class Passage
    {
        /// <summary>
        /// 文章主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>`
        /// 存储文章的本地路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 文章名字
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 摘要
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// 最近一次阅读时间
        /// </summary>
        public DateTime LastReadTime { get; set; }

        /// <summary>
        /// 上次阅读位置
        /// </summary>
        public string LastReadPlace { get; set; }

        /// <summary>
        /// 保留字段
        /// </summary>
        public string RemainWords { get; set; }

        /// <summary>
        /// 上次阅读页数
        /// </summary>
        public int Page { get; set; }
    }
}