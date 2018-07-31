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
            _viewModel.NewName = null;
            var res = await NewDialog.ShowAsync();
        }


        private void _viewModel_DicOpAction(object sender, CommandArgs args) {
            switch (args.command)
            {
                case "Open":
                    VisualStateManager.GoToState(this, "CompleteInfo", true);
                    break;
                case "Close":
                    VisualStateManager.GoToState(this, "BrifeInfo", true);
                    break;
                case "ReName":
                   
                    break;
                case "ReMove":
                    
                    break;

            }
        }

        private void _viewModel_DialogActions(object sender, CommandArgs args) {
            switch (args.parameter)
            {
                case "YES":
                    Debug.WriteLine(_viewModel.NewName);
                    break;
                case "NO":
                    Debug.WriteLine(_viewModel.NewName);
                    break;
            }
            NewDialog.Hide();
        }
        #endregion

        #region Private
        private void DicPage_Loaded(object sender, RoutedEventArgs e) {
            _viewModel = DataContext as DicPageViewModel;
            _viewModel.CommandActions += _viewModel_CommandActions;
            _viewModel.DialogActions += _viewModel_DialogActions;
            _viewModel.DicOpAction += _viewModel_DicOpAction;
        }

        private void DicPage_Unloaded(object sender, RoutedEventArgs e) {
            _viewModel.CommandActions -= _viewModel_CommandActions;
            _viewModel.DialogActions -= _viewModel_DialogActions;
            _viewModel.DicOpAction -= _viewModel_DicOpAction;
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
