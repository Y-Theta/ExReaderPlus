using ExReaderPlus.Manage.PassageManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Manage
{
    [TestClass]
    public class PassageManageTest
    {
        [TestMethod]
        public void SavaPassageInfoAndPassageTest()
        {
            var passage = new ExReaderPlus.Manage.PassageManager.Passage
            {
                Content = "Mr. Johnson had never been up in an aerophane before and he " +
                "had read a lot about air accidents, so one day when a friend offered to take him for a ride in his own small phane, " +
                "Mr. Johnson was very worried about accepting. Finally, however, his friend persuaded him that it was very safe, and Mr. Johnson boarded the plane.",
                HeadName = "test"
                
            };
           PassageManage.SavaPassageInfoAndPassage(passage, null);
        }
    }
}
