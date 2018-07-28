using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ExReaderPlus.Manage.PassageManager;
using ExReaderPlus.FileManage;

namespace ExReaderPlus.Serializer
{
    /// <summary>
/// 书架
/// </summary>
    public class Serializer
    {
        FileManage.FileManage fileManage=new FileManage.FileManage();
        /// <summary>
        /// 存
        /// </summary>
        /// <param name="passage"></param>
        /// <param name="str1"></param>
        /// <returns></returns>
        public  async Task<bool> serializer(Passage passage,string str1)
        {
            
//            BinaryFormatter binaryFormatter = new BinaryFormatter();
            StorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
            //            Debug.WriteLine(applicationFolder.Path);


            try
            {
                StorageFile saveFile =
                    await applicationFolder.CreateFileAsync(str1);
                FileIO.WriteTextAsync(saveFile, passage.Content);
                return true;
            }
            catch
            {
                return false;
            }

            
               
            
            //            FileStream stream = new FileStream("./file", FileMode.Create);
            //            binaryFormatter.Serialize(stream, passage);
        }
        
        /// <summary>
        /// 读
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
       public async Task<Passage> deserializer(string str)
        {

            Passage passage = new Passage();
            StorageFolder folder = ApplicationData.Current.RoamingFolder;
                StorageFile file = await folder.TryGetItemAsync(str) as StorageFile;
            if (file != null)
            {
                passage.Content = await FileIO.ReadTextAsync(file);
                //                BinaryFormatter binaryFormatter = new BinaryFormatter();

                //                passage1 = (Passage)binaryFormatter.Deserialize(stream);
                //                Console.WriteLine(people.Name);
//                Directory.Delete();
                passage.HeadName = "xxxx";
                return passage;
                
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 删
        /// </summary>
        /// <param name="str"></param>
        public async Task<bool> Delete(string str)
        {
            try
            {
                StorageFolder folder = ApplicationData.Current.RoamingFolder;
                StorageFile file = await folder.TryGetItemAsync(str) as StorageFile;
                await file.DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            } 
            
        }
    }

}
