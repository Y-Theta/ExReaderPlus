using ExReaderPlus.Manage.PassageManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Manage
{

    /// <summary>
    ///测试文章保存类
    /// ID即便被删除，也不会重复生成已经删除的ID
    /// </summary>
    [TestClass]
    public class PassageManageTest
    {
        /// <summary>
        /// 存储文章
        /// </summary>
        [TestMethod]
        public void SavaPassageInfoAndPassageTest()
        {
            var passage0 = new ExReaderPlus.Manage.PassageManager.Passage
            {
                Content = "Mr. Johnson had never been up in an aerophane before and he " +
                "had read a lot about air accidents, so one day when a friend offered to take him for a ride in his own small phane, " +
                "Mr. Johnson was very worried about accepting. Finally, however, his friend persuaded him that it was very safe, and Mr. Johnson boarded the plane.",
                HeadName = "test0"

            };

            // var passage1 = new ExReaderPlus.Manage.PassageManager.Passage
            // {
            //     Content = "First Flight Mr. Johnson had never been up in an aerophane before and he had read a lot about air accidents",
            //     HeadName="test1"

            // };
            PassageManage.SavaPassageInfoAndPassage(passage0);
            //PassageManage.SavaPassageInfoAndPassage(passage1);           
        }


        /// <summary>
        /// 根据文章内容判断文章是否存在
        /// </summary>
        [TestMethod]
        public void ifExistTest()
        {
            var passage0 = new ExReaderPlus.Manage.PassageManager.Passage
            {
                Content = "Mr. Johnson had never been up in an aerophane before and he " +
               "had read a lot about air accidents, so one day when a friend offered to take him for a ride in his own small phane, " +
               "Mr. Johnson was very worried about accepting. Finally, however, his friend persuaded him that it was very safe, and Mr. Johnson boarded the plane.",
                HeadName = "test0"

            };
            Assert.AreEqual("test0", PassageManage.ifExsist(passage0).Name);
        }


        /// <summary>
        /// 测试获取所有文章信息书架
        /// </summary>
        [TestMethod]
        public void GetAllPassagesInfoTest()
        {
            var passageInfoList = PassageManage.GetAllPassagesInfo();
            Assert.AreEqual("test0", passageInfoList.FirstOrDefault(p => p.Id.Equals(1)).Name);
            Assert.AreEqual("test1", passageInfoList.FirstOrDefault(p => p.Id.Equals(2)).Name);
        }


        /// <summary>
        /// 测试删除文章
        ///  TODO修改成稳定通过的测试
        /// </summary>
        [TestMethod]
        public void DeleteDeletePassageInfoAndPassageTest()
        {
            var passage0 = new ExReaderPlus.Manage.PassageManager.Passage
            {
                Content = "Mr. Johnson had never been up in an aerophane before and he " +
                 "had read a lot about air accidents, so one day when a friend offered to take him for a ride in his own small phane, " +
                 "Mr. Johnson was very worried about accepting. Finally, however, his friend persuaded him that it was very safe, and Mr. Johnson boarded the plane.",
                HeadName = "test3"

            };
            PassageManage.SavaPassageInfoAndPassage(passage0);

            Assert.AreEqual(1, PassageManage.DeletePassageInfoAndPassage(new UserDictionary.Passage
            {
                Id = 4
            }));
        }

        [TestMethod]
        public async Task GetPassageTest()
        {
            //var passageInfo = new UserDictionary.Passage
            //{
            //    Id = 2
            //};
            //var p =  await PassageManage.GetPassage(passageInfo);
            //Assert.AreEqual("", p.Content);
        }
    }
}
