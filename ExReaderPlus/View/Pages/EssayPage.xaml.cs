using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ExReaderPlus.ViewModels;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ExReaderPlus.View.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EssayPage : Page {

        private EssayPageViewModel viewModel;
        private RichTextBox richTextBox;
        public EssayPage() {
            this.InitializeComponent();
            this.Loaded += EssayPage_Loaded;
            this.Unloaded += EssayPage_Unloaded;
        }

        private void EssayPage_Unloaded(object sender, RoutedEventArgs e) {
            viewModel.PassageLoaded -= EssayPage_PassageLoaded;
        }

        private void EssayPage_Loaded(object sender, RoutedEventArgs e) {
            viewModel = DataContext as EssayPageViewModel;
            viewModel.PassageLoaded += EssayPage_PassageLoaded;
        }

        private async void EssayPage_PassageLoaded(object sender, EventArgs e) {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                TextView.Document.SetText(Windows.UI.Text.TextSetOptions.None, viewModel.TempPassage.Content);
            });
        }

        //private void Text_OnClick(object sender, RoutedEventArgs e)
        //{
        //    FileManage.FileManage fileManage = new FileManage.FileManage();
        //    fileManage.Win2DTask(viewModel.TempPassage.Content);
        //}
    }
}
