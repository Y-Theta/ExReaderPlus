using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ExReaderPlus.Manage.PassageManager;
using Windows.Storage;
using System.IO;
using System.Diagnostics;
using System.Numerics;
using Windows.ApplicationModel.Core;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using ExReaderPlus.View;
using ExReaderPlus.ViewModels;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;


namespace ExReaderPlus.FileManage {
   /// <summary>
   /// File
   /// </summary>
   

    public class FileManage
    {
        private EssayPageViewModel viewModel;
        private static FileManage _instence;
        
        public static FileManage Instence {
           get {
                if (_instence == null)
                    _instence = new FileManage();
                return _instence;
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="passage"></param>
        public async void SerializeFile(UserDictionary.Passage passage)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(UserDictionary.Passage));
            StorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await applicationFolder.CreateFileAsync("Save", CreationCollisionOption.GenerateUniqueName);
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // write to file
                var stream = await file.OpenStreamForWriteAsync();
                Debug.WriteLine("write stream: " + stream.ToString());
                serializer.WriteObject(stream, passage);

                Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
               
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
        /// <summary>
        /// *new 导入文件
        /// </summary>
        /// <returns></returns>
        public async Task<Passage> OpenFile()
        {
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

        /// <summary>
        /// toast通知
        /// </summary>
        /// <param name="title"></param>
        /// <param name="stringContent"></param>
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

        /// <summary>
        /// 用于网页分享功能
        /// </summary>
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

        /// <summary>
        /// win2d对图片写字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public async Task Win2DTask(string str, string strTime, string strNum)
        {
            var pick = new FileOpenPicker();
            pick.FileTypeFilter.Add(".jpg");
            pick.FileTypeFilter.Add(".png");

            var file = await pick.PickSingleFileAsync();
            var duvDbecdgiu =
                await CanvasBitmap.LoadAsync(new CanvasDevice(true), await file.OpenAsync(FileAccessMode.Read));
            var canvasRenderTarget = new CanvasRenderTarget(duvDbecdgiu, duvDbecdgiu.Size);
            var str1 = str.Substring(0, str.Length / 2);
            char c = str1[str1.Length - 1];
            if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z')
            {
                str1.Insert(str1.Length, "-");
            }
            else
            {

            }
            var str2 = str.Substring(str.Length / 2);
            //            char[]strLast = str1.Substring(str1.Length).ToCharArray();


            using (var dc = canvasRenderTarget.CreateDrawingSession())//用后则需撤销
            {

                ///先将图片读取
                dc.DrawImage(duvDbecdgiu);
                ///写图片
                dc.DrawText("Total Words :" + strNum,
                    760, 278, 330, 60,
                    Colors.Blue, new CanvasTextFormat()
                    {
                        FontSize = 30


                    });
                dc.DrawText("Time To Read :" + strTime,
                    760, 338, 330, 60,
                    Colors.Blue, new CanvasTextFormat()
                    {
                        FontSize = 30


                    });


                dc.DrawText(str1, 26, 408, 538, 1358, Colors.Black, new CanvasTextFormat()
                {
                    FontSize = 24
                });

                dc.DrawText(str2, 626, 458, 544, 1080, Colors.Black, new CanvasTextFormat()
                {
                    FontSize = 24
                });
            }


            string desiredName = "Share" + ".png";
            StorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
            //            Debug.WriteLine(applicationFolder.Path);

            StorageFile saveFile = await applicationFolder.CreateFileAsync(desiredName, CreationCollisionOption.GenerateUniqueName);

            await canvasRenderTarget.SaveAsync(await saveFile.OpenAsync(FileAccessMode.ReadWrite), CanvasBitmapFileFormat.Png);
            // 把图片展现出来
            var fileStream = await saveFile.OpenReadAsync();
            var bitmap = new BitmapImage();
            //bitmap是文件生成的位图
            await bitmap.SetSourceAsync(fileStream);
            //todo 将bitmap赋给image并显示
            


            ShowToastNotification("图片已保存成功","请预览");

           

//            
        }

//        public async void NewPage()
//        {
//
//            CoreApplicationView newView = CoreApplication.CreateNewView();
//
//            int newViewId = 0;
//
//            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
//
//            {
//
//                Frame frame = new Frame();
//
//                frame.Navigate(typeof(SharePage), null);
//
//                Window.Current.Content = frame;
//
//                // You have to activate the window in order to show it later.
//
//                Window.Current.Activate();
//
//
//
//                newViewId = ApplicationView.GetForCurrentView().Id;
//
//            });
//
//            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
//        }





    }
}
