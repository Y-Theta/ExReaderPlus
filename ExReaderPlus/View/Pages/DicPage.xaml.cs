using ExReaderPlus.View.Commands;
using ExReaderPlus.ViewModels;
using System;
using System.Diagnostics;
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
            //DefaultDialog dia = new DefaultDialog();
            //ContentControl s = new ContentControl();
            //dia.Background = new SolidColorBrush(Color.FromArgb(255, 0, 60, 180));
            //s.Style = Resources["AddDicContent"] as Style;
            //dia.Content = s;
            //dia.PrimaryButtonText = "\uE701";
            //dia.PrimaryButtonCommand = new CommandBase(obj => {

            //});
            //dia.PrimaryButtonStyle = App.Current.Resources["DialogButtonStyle"] as Style;
            //dia.PrimaryButtonMargin = new Thickness(0, 0, 0, 0);

            //var dd = await dia.ShowAsync(ContentDialogPlacement.Popup);
            //Debug.WriteLine(dia.ActualHeight);
            
            var res = await NewDialog.ShowAsync();
            
        }
        #endregion

        #region PrivateEvents
        private void DicPage_Loaded(object sender, RoutedEventArgs e) {
            _viewModel = DataContext as DicPageViewModel;
            _viewModel.CommandActions += _viewModel_CommandActions;
          //  (App.Current.Resources["OverSettingService"] as OverSettingService).SetStateBarButtonFg(Color.FromArgb(255, 255, 255, 255));
        }

        private void DicPage_Unloaded(object sender, RoutedEventArgs e) {
            _viewModel.CommandActions -= _viewModel_CommandActions;

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
