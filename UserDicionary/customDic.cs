using System;
using System.Collections.Generic;
using System.Text;

namespace UserDic
{
    /// <summary>
    /// 用户自定义词典类
    /// </summary>
    public class customDic
    {
        /// <summary>
        /// 主键id编程词的意思
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 词典的名字
        /// </summary>
        public string dicName { get; set; }
        public string word { get; set; }           //单词
        public string translation { get; set; }    //单词释义
        public int classification { get; set; }    //单词分类
        public int yesorNo { get; set; }      //单词掌握情况
        private string phonetic { get; set; }    //音标,新增
    }
}
