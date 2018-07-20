using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ExReaderPlus.WordsManager;
using ExReaderPlus.Manage.PassageManager;
using Windows.Storage;
using System.IO;
using System.Diagnostics;
using Windows.Storage.Pickers;
using Windows.System;
using ExReaderPlus.Manage.ReaderManager;
using Windows.UI.Notifications;


namespace ExReaderPlus.FileManage
{
    //汤浩工作空间
    //本类实现将 #工程文件# 涉及到的 #类数据# 打包与解包，实现序列化、反序列化，实现导入、导出工程文件 （自定义文件名 .xread）
    //附操作 MainReader.xaml.cs

    public class FileManage
    {
        private static FileManage _instence;
        public static FileManage Instence {
           get {
                if (_instence != null)
                    _instence = new FileManage();
                return _instence;
            }
        }

        //序列化
        public async void SerializeFile(ReaderManage reader)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(ReaderManage));
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("exReader文件", new List<string>() { ".xread" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "xPassage";
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // write to file
                var stream = await file.OpenStreamForWriteAsync();
                Debug.WriteLine("write stream: " + stream.ToString());
                serializer.WriteObject(stream, reader);

                Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                ShowToastNotification("exReader提示", "成功导出工程文件!");
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    //.textBlock.Text = "File " + file.Name + " was saved.";
                }
                else
                {
                    //this.textBlock.Text = "File " + file.Name + " couldn't be saved.";
                }
            }
            else
            {
                //this.textBlock.Text = "Operation cancelled.";
            }
        }

        //反序列化
        public async Task<Passage> DeSerializeFile()
        {

            DataContractSerializer deserializer = new DataContractSerializer(typeof(ReaderManage));
            
            Passage passage = new Passage();
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            picker.FileTypeFilter.Add(".txt");
            
            // TODO:
            //picker.FileTypeFilter.Add(".pdf");

            StorageFile storageFile = await picker.PickSingleFileAsync();
            if (storageFile != null)
            {
                var stream = await storageFile.OpenStreamForReadAsync();

                passage.Content = await FileIO.ReadTextAsync(storageFile);
                passage.HeadName = storageFile.DisplayName;


                return passage;
            }
            else
            {
                return null;
            }

        }

        //显示Toast通知
        private void ShowToastNotification(string title, string stringContent)
        {
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(title));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(stringContent));
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            ToastNotification toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            ToastNotifier.Show(toast);
        }


        public async void ShareData()
        {


            //从文本框获取文章内容
            var uri = new Uri(@"https://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?url=http%3A%2F%2Fv.ExreaderPlus.com%2Fvideo%2F3250296&desc=I+find+a+pretty+good+passage+in+ExreaderPlus+you+should+not+miss+it+");

            var success = await Launcher.LaunchUriAsync(uri);
            if (success)
            {
                // 如果你感兴趣，可以在成功启动后在这里执行一些操作。

                ShowToastNotification("ExReaderPlus提示", "分享成功");

            }
            else
            {
                // 如果你感兴趣，可以在这里处理启动失败的一些情况。
                ShowToastNotification("ExReaderPlus提示", "分享失败");
            }

        }

    }
}
