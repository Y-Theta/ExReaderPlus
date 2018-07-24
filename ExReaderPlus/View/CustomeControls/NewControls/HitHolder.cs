using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace ExReaderPlus.View
{
    public sealed class HitHolder : Control
    {
        #region Properties
        /// <summary>
        /// 鼠标悬浮颜色
        /// </summary>
        public Brush PointBrush {
            get { return (Brush)GetValue(PointBrushProperty); }
            set { SetValue(PointBrushProperty, value); }
        }
        public static readonly DependencyProperty PointBrushProperty =
            DependencyProperty.Register("PointBrush", typeof(Brush), 
                typeof(HitHolder), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// 鼠标点击颜色
        /// </summary>
        public Brush PressBrush {
            get { return (Brush)GetValue(PressBrushProperty); }
            set { SetValue(PressBrushProperty, value); }
        }
        public static readonly DependencyProperty PressBrushProperty =
            DependencyProperty.Register("PressBrush", typeof(Brush), 
                typeof(HitHolder), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        #endregion

        #region Methods
        protected override void OnPointerEntered(PointerRoutedEventArgs e) {
            base.OnPointerEntered(e);
            VisualStateManager.GoToState(this, "PointerOver", false);
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e) {
            base.OnPointerExited(e);
            VisualStateManager.GoToState(this, "Normal", false);
        }

        private void HitHolder_Loaded(object sender, RoutedEventArgs e) {
            VisualStateManager.GoToState(this, "Normal", false);
        }
        #endregion

        public HitHolder()
        {
            DefaultStyleKey = typeof(HitHolder);
            Loaded += HitHolder_Loaded;
        }
    }
}
