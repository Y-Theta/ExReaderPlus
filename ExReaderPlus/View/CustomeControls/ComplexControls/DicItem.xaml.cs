using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace ExReaderPlus.View {
    public sealed partial class DicItem : UserControl {
        /// <summary>
        /// 测得的宽度，用于数据绑定
        /// </summary>
        public double MessureWidth {
            get { return (double)GetValue(MessureWidthProperty); }
            set { SetValue(MessureWidthProperty, value); }
        }
        public static readonly DependencyProperty MessureWidthProperty =
            DependencyProperty.Register("MessureWidth", typeof(double), 
                typeof(DicItem), new PropertyMetadata(0.0));

        public DicItem() {
            InitializeComponent();
            SizeChanged += DicItem_SizeChanged;
        }

        private void DicItem_SizeChanged(object sender, SizeChangedEventArgs e) {
            MessureWidth = e.NewSize.Width;
        }
    }
}
