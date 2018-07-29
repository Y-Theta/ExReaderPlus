using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ExReaderPlus.WordsManager;
using Microsoft.EntityFrameworkCore;
using UserDictionary;

namespace ExReaderPlus.Manage.PassageManager
{
    /// <summary>
    /// 文章类
    /// </summary>
    public class Passage
    {
        public string HeadName { get; set; }
        public string Content { get; set; }
    }

    /// <summary>
    /// 文章条目和文章内容的关联类
    /// 实现通过文章条目打开文件，关闭文件
    /// UserDictionary.Passage即是PassageInfo类
    /// </summary>
    public static class PassageManage
    {

        /// <summary>
        /// 存储文章到书架,并且把文章缓存到路径
        /// 传入参数
        /// 【1】文章(包含文章内容和文章标题)
        /// 【2】文章信息(新导入的文章传入为空)
        /// 传出参数
        /// 异常 -1 
        /// 成功 1
        /// </summary>
        /// <param name="passage"></param>
        /// <returns></returns>
        public static int SavaPassageInfoAndPassage(ExReaderPlus.Manage.PassageManager.Passage passage,UserDictionary.Passage passageInfo)
        {
            using(var db=new DataContext())
            {
                db.Database.Migrate();
                if (passageInfo == null)//如果文章从未保存过，则传入文章信息为空
                {
                    passageInfo = new UserDictionary.Passage();
                    passageInfo.Name = passage.HeadName;                
                    var time = new DateTime();
                    time = System.DateTime.Now;
                    passageInfo.LastReadTime = time;
                    if (passage.Content.Length > 32)//防止文章内容过短引发异常
                    {
                        passageInfo.Abstract = passage.Content.Substring(0, 32) + "...";
                    }
                    else
                    {
                        passageInfo.Abstract = passage.Content;
                    }                    
                    db.Passages.Add(passageInfo);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        return -1;
                    }
                    finally
                    {
                        db.Database.CloseConnection();
                    }
                    db.Database.Migrate();
                    var savePassageInfo=db.Passages//用于传入保存函数的文章信息，主要用来获取数据库自动生成的id
                        .Where(p => p.Name.Equals(passageInfo.Name))
                        .Where(p => p.Abstract.Equals(passageInfo.Abstract)).FirstOrDefault();//通过文章名字和文章摘要查找文章来获取新建的ID
                    SavaPassageInfoAndPassage(passage, savePassageInfo);
                    return 1;
                }
                else//如果已经存在文件
                {

                }
                return -1;
            }
        }

        
    }

}
