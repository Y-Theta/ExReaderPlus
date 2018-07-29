using System;
using ExReaderPlus.Serializer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExReaderPlus.Manage.PassageManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ExReaderPlus.FileManage;
namespace UnitTest.Manage
{
    [TestClass]
    public class FileManageTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OpenFileTest()
        {
            
            Passage passage= null;
            Serializer serializer=new Serializer();
            passage= await serializer.deserializer("Save.txt");
            while (passage is null) ;
       
            Assert.AreEqual(passage.Content,"Hello world");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        
        public async Task SaveFileTask()
        {
            Passage passage=new Passage();
            Passage passage1 = new Passage();
            passage.Content = "Hello world";
            Serializer serializer = new Serializer();
            await serializer.serializer(passage, "Hi.txt");
            passage1 = await serializer.deserializer("Hi.txt");
            while (passage1 is null) ;

            Assert.AreEqual(passage1.Content, "Hello world");


        }
    }
}
