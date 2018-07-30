using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExReaderPlus.Manage.PassageManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ExReaderPlus.FileManage;
using ExReaderPlus.PassageIO;
using UserDictionary;

namespace UnitTest.Manage
{
    [TestClass]
    public class FileManageTest
    {
        /// <summary>
        /// OpenFileTest
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OpenFileTest()
        {

           var passageIO=new PassageIO();
           var passageInfo=new UserDictionary.Passage();
            var passage = new ExReaderPlus.Manage.PassageManager.Passage();
            passageInfo.Id = 2;
            passage.Content = "Hello world";
                            
            Assert.AreEqual(true, await passageIO.SavaPassage(passage, passageInfo));
        }

        /// <summary>
        /// ContentTest & SaveTest
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        
        public async Task SaveFileTask()
        {
            var passageIO = new PassageIO();
            var passageInfo = new UserDictionary.Passage();
            passageInfo.Id = 1;
            
            
            var passage = new ExReaderPlus.Manage.PassageManager.Passage();
            passage.Content = "Hello world";
            await passageIO.SavaPassage(passage, passageInfo);
            var passage1=new ExReaderPlus.Manage.PassageManager.Passage();
             passage1 = await passageIO.ReadPassage(passageInfo);

            Assert.AreEqual(passage1.Content, "Hello world");


        }
    }
}
