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
    ///  TODO 待完善
    /// </summary>
    public static class PassageManage
    {
        /// <summary>
        /// 相当于宏定义摘要的长度
        /// </summary>
        private static int ABSTRACT_SIZE = 32;


        /// <summary>
        /// 得到摘要
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string getAbstract(string content)
        {
            if (content.Length > ABSTRACT_SIZE)//防止文章内容过短引发异常
            {
                return content.Substring(0, ABSTRACT_SIZE) + "...";
            }
            else
            {
                return content;
            }
        }

        /// <summary>
        /// 存储文章到书架,并且把文章缓存到路径
        /// 传入参数
        /// 【1】文章(包含文章内容和文章标题)
        /// 传出参数
        /// 异常 -1 
        /// 成功 1
        /// 有文件、数据库操作，建议异步
        ///  FIXME TODO
        /// </summary>
        /// <param name="passage"></param>
        /// <returns></returns>
        public static int SavaPassageInfoAndPassage(ExReaderPlus.Manage.PassageManager.Passage passage)
        {
            using (var db = new DataContext())
            {
                var returnPassageInfo = ifExsist(passage);
                if (returnPassageInfo == null)//如果文章不存在,就新建文章条目
                {
                    db.Database.Migrate();
                    var passageInfo = new UserDictionary.Passage();
                    passageInfo.Name = passage.HeadName;
                    passageInfo.LastReadTime = System.DateTime.Now;//写入时间
                    passageInfo.Abstract = getAbstract(passage.Content);
                    passageInfo.RemainWords = passage.Content.Length.ToString();
                    //C#中int最大为2^32-1不考虑超过的情况
                    db.Passages.Add(passageInfo);//存入文件信息
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
                    var savePassageInfo = db.Passages//用于传入保存函数的文章信息，主要用来获取数据库自动生成的id
                        .Where(p => p.Name.Equals(passageInfo.Name))
                        .Where(p => p.Abstract.Equals(passageInfo.Abstract)).FirstOrDefault();//通过文章名字和文章摘要查找文章来获取新建的ID
                    ExReaderPlus.PassageIO.PassageIO passageIO = new ExReaderPlus.PassageIO.PassageIO();
                    try
                    {
                        passageIO.SavaPassage(passage, savePassageInfo);//根据获取的唯一ID保存文件
                        return 1;
                    }
                    catch
                    {
                        return -1;
                    }
                }
                else
                {
                    db.Passages.Find(returnPassageInfo.Id).LastReadTime = System.DateTime.Now;
                    try
                    {
                        db.SaveChanges();
                        return 1;
                    }
                    catch
                    {
                        return -1;
                    }
                    finally
                    {
                        db.Database.CloseConnection();
                    }
                }
            }
        }


        /// <summary>
        /// 判断数据库中是否存在当前内容，建议改为异步操作
        /// 返回参数:
        /// 存在 返回UserDictionary.Passage
        /// 不存在返回 null
        /// </summary>
        /// <param name="passage"></param>
        /// <returns></returns>
        public static UserDictionary.Passage ifExsist(ExReaderPlus.Manage.PassageManager.Passage passage)
        {
            using (var db = new DataContext())
            {
                db.Database.Migrate();
                string _abstract;
                _abstract = getAbstract(passage.Content);
                var passages = db.Passages;//读入内存增加查询速度
                db.Database.CloseConnection();
                var savePassageInfo = passages//用于传入保存函数的文章信息，主要用来获取数据库自动生成的id
                                    .Where(p => p.Name.Equals(passage.HeadName))
                                    .Where(p => p.Abstract.Equals(_abstract))
                                    .Where(p => p.RemainWords.Equals(passage.Content.Length.ToString()))
                                    .FirstOrDefault();//通过文章名字和文章摘要查找文章来获取新建的ID         
                return savePassageInfo;
            }
        }

        /// <summary>
        /// 将所有文章信息迭代出来，用于显示文章信息(包括文章名字Name、文章摘要Abstract、文章上次阅读时间）
        /// </summary>
        /// <returns></returns>
        public static List<UserDictionary.Passage> GetAllPassagesInfo()
        {
            using(var db=new DataContext())
            {
                db.Database.Migrate();
                var passageList = db.Passages.ToList();
                db.Database.CloseConnection();
                return passageList;
            }
        }


        /// <summary>
        /// 传入参数：文章信息
        /// 把一篇文章从书架删除，并且删除缓存文件
        /// 成功返回 1 
        /// 失败返回 0
        /// 异常返回 -1
        /// </summary>
        /// <returns></returns>
        public static int DeletePassageInfoAndPassage(UserDictionary.Passage passageInfo)
        {
            using(var db=new DataContext())
            {
                db.Passages.Remove(
                    db.Passages.FirstOrDefault(p => p.Id.Equals(passageInfo.Id))
                    );
                try {
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    return -1;
                }
                finally
                {
                    db.Database.CloseConnection();
                }
            }
        }

        /// <summary>
        /// 【只有这个是异步的】
        /// </summary>
        /// <returns></returns>
        public static Task<ExReaderPlus.Manage.PassageManager.Passage> GetPassage(UserDictionary.Passage passageInfo) {
            ExReaderPlus.PassageIO.PassageIO passageIO = new ExReaderPlus.PassageIO.PassageIO();
            var passage = passageIO.ReadPassage(passageInfo);
            return passage;
        }

        public static ExReaderPlus.Manage.PassageManager.Passage GetPassage()
        {
            return null;
        }
    }

}  