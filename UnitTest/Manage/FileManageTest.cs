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
        [TestMethod]
        public async void OpenFileTest()
        {

            Passage passage=new Passage();
            Serializer serializer=new Serializer();
            passage= await serializer.deserializer("Save");
            Assert.AreEqual(passage.Content,"Hello world");
        }
    }
}
