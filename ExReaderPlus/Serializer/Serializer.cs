using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ExReaderPlus.Manage.PassageManager;


namespace ExReaderPlus.Serializer
{
    public class Serializer
    {
        public async void serializer(Passage passage)
        {
            string name = "Save" + ".txt";
//            BinaryFormatter binaryFormatter = new BinaryFormatter();
            StorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
            //            Debug.WriteLine(applicationFolder.Path);
            
            StorageFile saveFile = await applicationFolder.CreateFileAsync(name, CreationCollisionOption.GenerateUniqueName);
            FileIO.WriteTextAsync(saveFile, passage.Content);
            //            FileStream stream = new FileStream("./file", FileMode.Create);
            //            binaryFormatter.Serialize(stream, passage);
        }

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
                passage.HeadName = "xxxx";
                return passage;
            }
            else
            {
                return null;
            }
        }

    }
}
