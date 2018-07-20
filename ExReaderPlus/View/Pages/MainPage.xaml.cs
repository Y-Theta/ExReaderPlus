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
using Windows.UI.Xaml.Navigation;
using ExReaderPlus.ViewModels;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Xaml.Media.Imaging;

namespace ExReaderPlus.View.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page {
        RenderTargetBitmap _frameclip;

        public MainPage() {
            this.InitializeComponent();
            _frameclip = new RenderTargetBitmap();
          //  InitializeFrostedGlass(ArcLayer);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            Window.Current.SetTitleBar(TitleBarTouch);
            var viewmodel = (MainPageViewModel)DataContext;
            MainFrame.Navigate(typeof(EssayPage));
            MainFrame.Navigating += MainFrame_Navigating;
            await _frameclip.RenderAsync(MainFrame);
        }

        private void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e) {
            
        }

        private void ArcLayer_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args) {
            
        }

        private void ArcLayer_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args) {
            using (var session = args.DrawingSession)
            {
                var blur = new GaussianBlurEffect
                {
                    BlurAmount = 50.0f, // increase this to make it more blurry or vise versa.
                                        //Optimization = EffectOptimization.Balanced, // default value
                                        //BorderMode = EffectBorderMode.Soft // default value
                  
                };

               // session.DrawImage(blur, new Rect(0, 0, sender.ActualWidth, sender.ActualHeight), 0.9f);
            }
        }
    }
}
