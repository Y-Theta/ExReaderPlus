using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace ExReaderPlus.View {
    public sealed partial class VocabularyHeader : UserControl {

        /// <summary>
        /// 鼠标悬浮颜色
        /// </summary>
        public Brush PointBrush {
            get { return (Brush)GetValue(PointBrushProperty); }
            set { SetValue(PointBrushProperty, value); }
        }
        public static readonly DependencyProperty PointBrushProperty =
            DependencyProperty.Register("PointBrush", typeof(Brush),
                typeof(VocabularyHeader), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// 鼠标点击颜色
        /// </summary>
        public Brush PointMask {
            get { return (Brush)GetValue(PointMaskProperty); }
            set { SetValue(PointMaskProperty, value); }
        }
        public static readonly DependencyProperty PointMaskProperty =
            DependencyProperty.Register("PointMask", typeof(Brush),
                typeof(VocabularyHeader), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));


        protected override void OnPointerEntered(PointerRoutedEventArgs e) {
            base.OnPointerEntered(e);
            VisualStateManager.GoToState(this, "PointerOver", false);
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e) {
            base.OnPointerExited(e);
            VisualStateManager.GoToState(this, "Normal", false);
        }

        private void VocabularyHeader_Loaded(object sender, RoutedEventArgs e) {
        }

        public VocabularyHeader() {
            InitializeComponent();
            Loaded += VocabularyHeader_Loaded;
        }
    }
}
