using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ExReaderPlus.Manage;
using ExReaderPlus.FileManage;
using UserDictionary;


namespace ExReaderPlus.PassageIO
{

    /// <summary>
    /// 文章增删读
    /// </summary>
    public class PassageIO
    {


        FileManage.FileManage fileManage=new FileManage.FileManage();
        /// <summary>
        /// 存一篇文章
        /// 传入参数，文章类和文章序列
        /// </summary>
        /// <param name="passage"></param>
        /// <param name="str1"></param>
        /// <returns></returns>
        public  async Task<bool> SavaPassage(Manage.PassageManager.Passage passage,UserDictionary.Passage passageInfo)
        {
           
            StorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile saveFile =
                    await applicationFolder.CreateFileAsync(passageInfo.Id.ToString());
                await FileIO.WriteTextAsync(saveFile, passage.Content);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 读一篇文章内容
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
       public async Task<Manage.PassageManager.Passage> ReadPassage(UserDictionary.Passage passage)
        {

            Manage.PassageManager.Passage p = new Manage.PassageManager.Passage();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.TryGetItemAsync(passage.Id.ToString()) as StorageFile;
            if (file != null)
            {
                p.Content = await FileIO.ReadTextAsync(file);
                p.HeadName = passage.Name;
                return p;              
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 删除一篇文章
        /// </summary>
        /// <param name="str"></param>
        public async Task<bool> DeletePassage(UserDictionary.Passage passage)
        {
            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.TryGetItemAsync(passage.Id.ToString()) as StorageFile;
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
