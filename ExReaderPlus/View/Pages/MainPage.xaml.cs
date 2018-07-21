using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using ExReaderPlus.FileManage;
using Windows.UI.Xaml.Navigation;
using ExReaderPlus.ViewModels;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using ExReaderPlus.Manage.PassageManager;

namespace ExReaderPlus.View.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page {
       

        public MainPage() {
            this.InitializeComponent();
            this.Unloaded += MainPage_Unloaded;
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e) {
            MainFrame.Navigating -= MainFrame_Navigating;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            Window.Current.SetTitleBar(TitleBarTouch);
            var viewmodel = (MainPageViewModel)DataContext;
            viewmodel.OnNavigate += Viewmodel_OnNavigate;
            MainFrame.Navigate(typeof(EssayPage));
            MainFrame.Navigating += MainFrame_Navigating;
          //  await _frameclip.RenderAsync(MainFrame);
        }

        private void Viewmodel_OnNavigate(object sender, EventArgs e) {
            MainFrame.Navigate(sender.GetType(),null,new SuppressNavigationTransitionInfo());
        }

        private async void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e) {

        }

    }
}
