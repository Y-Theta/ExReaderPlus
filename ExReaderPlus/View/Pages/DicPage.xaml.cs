using ExReaderPlus.View.Commands;
using ExReaderPlus.ViewModels;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ExReaderPlus.View.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DicPage : Page {
        DicPageViewModel _viewModel;


        #region Methods
        private async void _viewModel_CommandActions(object sender, CommandArgs args) {
            DefaultDialog dia = new DefaultDialog();
            ContentControl s = new ContentControl();
            s.Style = App.Current.Resources["AddDicContent"] as Style;
            dia.Content = s;
            dia.PrimaryButtonText = "\uE701";
            dia.PrimaryButtonCommand = new CommandBase(obj => {

            });
            dia.PrimaryButtonStyle = App.Current.Resources["IconButtonStyle"] as Style;
            var dd = await dia.ShowAsync(ContentDialogPlacement.Popup);
        }
        #endregion

        #region PrivateEvents
        private void DicPage_Loaded(object sender, RoutedEventArgs e) {
            _viewModel = DataContext as DicPageViewModel;
            _viewModel.CommandActions += _viewModel_CommandActions;
        }

        private void DicPage_Unloaded(object sender, RoutedEventArgs e) {

        }
        #endregion

        #region Constructor
        public DicPage() {
            InitializeComponent();
            Loaded += DicPage_Loaded;
            Unloaded += DicPage_Unloaded;
            ((DicPageViewModel)DataContext).window = Window.Current;
        }



        #endregion

        private void Page_DragOver(object sender, DragEventArgs e) {
            e.DragUIOverride.IsGlyphVisible = false;
        }
    }
}
