using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace ExReaderPlus.View {

    public sealed class IconViewItem : RadioButton {
        #region Properties
        /// <summary>
        /// 图标类型
        /// </summary>
        public IconKind IconFormat {
            get { return (IconKind)GetValue(IconFormatProperty); }
            set { SetValue(IconFormatProperty, value); }
        }
        public static readonly DependencyProperty IconFormatProperty =
            DependencyProperty.Register("IconFormat", typeof(IconKind), 
                typeof(IconViewItem), new PropertyMetadata(IconKind.Icon));

        /// <summary>
        /// 头像路径
        /// </summary>
        public string ImageSource {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string),
                typeof(IconViewItem), new PropertyMetadata(null));

        /// <summary>
        /// 图标属性
        /// </summary>
        public string Icon {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), 
                typeof(IconViewItem), new PropertyMetadata(null));

        /// <summary>
        /// 图标字体样式
        /// </summary>
        public FontFamily IconFont {
            get { return (FontFamily)GetValue(IconFontProperty); }
            set { SetValue(IconFontProperty, value); }
        }
        public static readonly DependencyProperty IconFontProperty =
            DependencyProperty.Register("IconFont", typeof(FontFamily), 
                typeof(IconViewItem), new PropertyMetadata(null));

        /// <summary>
        /// 选中可见性
        /// </summary>
        public Visibility SelectIconVisibility {
            get { return (Visibility)GetValue(SelectIconLocProperty); }
            set { SetValue(SelectIconLocProperty, value); }
        }
        public static readonly DependencyProperty SelectIconLocProperty =
            DependencyProperty.Register("SelectIconVisibility", typeof(Visibility),
                typeof(IconViewItem), new PropertyMetadata(Visibility.Visible));


        #region IconStroke
        public Brush IconStroke {
            get { return (Brush)GetValue(IconStrokeProperty); }
            set { SetValue(IconStrokeProperty, value); }
        }
        public static readonly DependencyProperty IconStrokeProperty =
            DependencyProperty.Register("IconStroke", typeof(Brush),
                typeof(IconViewItem), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
        #endregion

        #endregion

        #region Methods
        protected override void OnPointerPressed(PointerRoutedEventArgs e) {
            base.OnPointerPressed(e);
        }
        #endregion

        #region Constructors
        public IconViewItem() {
            this.DefaultStyleKey = typeof(IconViewItem);
        }
        #endregion
    }

}
