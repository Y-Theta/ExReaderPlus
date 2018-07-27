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
using Windows.UI.Xaml.Markup;
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

        /// <summary>
        /// 鼠标提示
        /// </summary>
        public string Tooltip {
            get { return (string)GetValue(TooltipProperty); }
            set { SetValue(TooltipProperty, value); }
        }
        public static readonly DependencyProperty TooltipProperty =
            DependencyProperty.Register("Tooltip", typeof(string), 
                typeof(HitHolder), new PropertyMetadata(null));
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

    [ContentProperty(Name = "Content")]
    public sealed class HitContentholder : Control {
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
                typeof(HitContentholder), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// 鼠标点击颜色
        /// </summary>
        public Brush PressBrush {
            get { return (Brush)GetValue(PressBrushProperty); }
            set { SetValue(PressBrushProperty, value); }
        }
        public static readonly DependencyProperty PressBrushProperty =
            DependencyProperty.Register("PressBrush", typeof(Brush),
                typeof(HitContentholder), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// 内容
        /// </summary>
        public UIElement Content {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElement),
                typeof(HitContentholder), new PropertyMetadata(null));
        #endregion

        #region Override
        protected override void OnPointerPressed(PointerRoutedEventArgs e) {
            base.OnPointerPressed(e);
            VisualStateManager.GoToState(this, "Pressed", false);
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e) {
            base.OnPointerReleased(e);
            VisualStateManager.GoToState(this, "PointerOver", false);
        }

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

        #region Constructor
        public HitContentholder() {
            DefaultStyleKey = typeof(HitContentholder);
        }
        #endregion
    }
}
