using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UserDictionary
{
    public class Dictionary
    {
        /// <summary>
        /// 词典名字
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 词汇的总量
        /// </summary>
        public int TotalWordsNumber { get; set; }

        /// <summary>
        ///一个词典包含多个单词
        /// </summary>
        public List<DictionaryWord> DictionaryWords { get; set; }

    }
}
