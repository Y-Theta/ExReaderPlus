using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ExReaderPlus.ViewModels;
using ExReaderPlus.Baidu;
using System.Diagnostics;
using Windows.UI.Xaml.Navigation;
using ExReaderPlus.Manage.PassageManager;

namespace ExReaderPlus.View.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EssayPage : Page {


        public EssayPage() {
            InitializeComponent();
            Loaded += EssayPage_Loaded;
        }

        private void EssayPage_Loaded(object sender, RoutedEventArgs e) {
            var viewModel = App.Current.Resources["EssayPageViewModel"] as EssayPageViewModel;
            if (viewModel != null && viewModel.TempPassage != null)
                viewModel.LoadPassage(viewModel.TempPassage);
        }

    }
}
