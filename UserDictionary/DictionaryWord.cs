using System;
using System.Collections.Generic;
using System.Text;

namespace UserDictionary
{
    /// <summary>
    /// 关联表
    /// </summary>
    public class DictionaryWord
    {
        public int Id { get; set; }
        /// <summary>
        /// 此处不能写WordID(两个都大写)
        /// </summary>
        public string WordId { get; set; }

        public Word Word { get; set; }

        public string DictionaryId { get; set; }

        public Dictionary Dictionary { get; set; }
    }
}
