using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ExReaderPlus.ViewModels;
using Windows.UI.Xaml.Media.Animation;

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
            MainFrame.Navigating += MainFrame_Navigating;
            MainFrame.Navigated += MainFrame_Navigated;
          //  await _frameclip.RenderAsync(MainFrame);
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e) {
            
        }

        private void Viewmodel_OnNavigate(object sender, EventArgs e) {
            MainFrame.Navigate(sender.GetType(),null,new SuppressNavigationTransitionInfo());
        }

        private void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e) {
       
        }

    }
}
