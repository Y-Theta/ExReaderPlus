using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ExReaderPlus.ViewModels;
using ExReaderPlus.Baidu;
using System.Diagnostics;

namespace ExReaderPlus.View.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EssayPage : Page {

        private EssayPageViewModel viewModel;
        public EssayPage() {
            InitializeComponent();
            Loaded += EssayPage_Loaded;
            Unloaded += EssayPage_Unloaded;
        }

        private void EssayPage_Unloaded(object sender, RoutedEventArgs e) {
        }

        private void EssayPage_Loaded(object sender, RoutedEventArgs e) {
            viewModel = DataContext as EssayPageViewModel;
            if (viewModel.TempPassage != null)
                TextView.SetText(Windows.UI.Text.TextSetOptions.None, viewModel.TempPassage.Content);
        }


    }
}
