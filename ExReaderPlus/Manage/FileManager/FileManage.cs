using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Notifications;
using ExReaderPlus.Manage.PassageManager;

namespace ExReaderPlus.Manage.FileManager
{
    public class FileManage
    {
        //序列化
       

        //反序列化
        public async Task<Passage> DeSerializeFile()
        {

            DataContractSerializer deserializer = new DataContractSerializer(typeof(Passage));
           
            Passage passage = new Passage();
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            picker.FileTypeFilter.Add(".txt");
            picker.FileTypeFilter.Add(".pdf");

            StorageFile storageFile = await picker.PickSingleFileAsync();
            if (storageFile != null)
            {



                passage.Content = await FileIO.ReadTextAsync(storageFile);
                passage.HeadName = "xxxxx";




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
            var myblog = new Uri(@"https://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?url=http%3A%2F%2Fv.yinyuetai.com%2Fvideo%2F3250296&desc=wangwangwangwang");
            var success = await Launcher.LaunchUriAsync(myblog);
            if (success)
            {
                ShowToastNotification("exReader提示", "分享成功"); // 如果你感兴趣，可以在成功启动后在这里执行一些操作。
            }
            else
            {
                // 如果你感兴趣，可以在这里处理启动失败的一些情况。
            }

        }
    }
}
