using System;
using System.Collections.Generic;
using System.Text;

namespace UserDictionary
{
    /// <summary>
    /// 单词内容
    /// </summary>
    public class Word
    {
        /// <summary>
        /// 主键id=word
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 翻译
        /// </summary>
        public string Translation { get; set; } 

        /// <summary>
        /// 单词分类
        /// </summary>
        public string Tag { get; set; }    
        
        /// <summary>
        /// 单词掌握情况
        /// </summary>
        public int YesorNo { get; set; }      

        /// <summary>
        /// 音标
        /// </summary>
        public string Phonetic { get; set; }

        /// <summary>
        /// 一个单词可以包含在多个词典里边
        /// </summary>
        public List<Dictionary> Dictionaries { get; set; }
    }
}
