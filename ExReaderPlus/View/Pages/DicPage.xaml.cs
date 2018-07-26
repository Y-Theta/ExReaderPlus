using ExReaderPlus.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace ExReaderPlus.View.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DicPage : Page {
        DicPageViewModel _viewModel;
        public DicPage() {
            InitializeComponent();
            ((DicPageViewModel)DataContext).window = Window.Current;
        }
    }
}
