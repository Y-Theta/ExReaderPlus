using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ExReaderPlus.ViewModels;
using ExReaderPlus.Baidu;

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
            viewModel.PassageLoaded -= EssayPage_PassageLoaded;
        }

        private void EssayPage_Loaded(object sender, RoutedEventArgs e) {
            viewModel = DataContext as EssayPageViewModel;
            viewModel.PassageLoaded += EssayPage_PassageLoaded;
            TextView.WordSelect += TextView_WordSelect;
        }

        private async void TextView_WordSelect(object sender, EventArgs e) {
            string s ="";
            var d = new Translate();
            if (sender.ToString() != null && sender.ToString() != "")
            {
                d.Text = sender.ToString();
                s = d.GetResult();
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    Return.Text = s;
                });
            }
            
        }

        private async void EssayPage_PassageLoaded(object sender, EventArgs e) {

            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                TextView.Document.SetText(Windows.UI.Text.TextSetOptions.None, viewModel.TempPassage.Content);
            });
        }
    }
}
