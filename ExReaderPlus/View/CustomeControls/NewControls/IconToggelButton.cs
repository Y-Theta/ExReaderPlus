using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace ExReaderPlus.View {
    public sealed class IconToggelButton : ToggleButton
    {
        #region Properties
        public Brush PointBrush {
            get { return (Brush)GetValue(PointBrushProperty); }
            set { SetValue(PointBrushProperty, value); }
        }
        public static readonly DependencyProperty PointBrushProperty =
            DependencyProperty.Register("PointBrush", typeof(Brush),
                typeof(IconToggelButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public Brush PressBrush {
            get { return (Brush)GetValue(PressBrushProperty); }
            set { SetValue(PressBrushProperty, value); }
        }
        public static readonly DependencyProperty PressBrushProperty =
            DependencyProperty.Register("PressBrush", typeof(Brush),
                typeof(IconToggelButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public Brush CheckedBrush {
            get { return (Brush)GetValue(CheckedBrushProperty); }
            set { SetValue(CheckedBrushProperty, value); }
        }
        public static readonly DependencyProperty CheckedBrushProperty =
            DependencyProperty.Register("CheckedBrush", typeof(Brush),
                typeof(IconToggelButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public string ForeIcon {
            get { return (string)GetValue(ForeIconProperty); }
            set { SetValue(ForeIconProperty, value); }
        }
        public static readonly DependencyProperty ForeIconProperty =
            DependencyProperty.Register("ForeIcon", typeof(string),
                typeof(IconToggelButton), new PropertyMetadata(null));

        public string BackIcon {
            get { return (string)GetValue(BackIconProperty); }
            set { SetValue(BackIconProperty, value); }
        }
        public static readonly DependencyProperty BackIconProperty =
            DependencyProperty.Register("BackIcon", typeof(string),
                typeof(IconToggelButton), new PropertyMetadata(null));

        public Visibility ToogelIdVis {
            get { return (Visibility)GetValue(ToogelIdVisProperty); }
            set { SetValue(ToogelIdVisProperty, value); }
        }
        public static readonly DependencyProperty ToogelIdVisProperty =
            DependencyProperty.Register("ToogelIdVis", typeof(Visibility), 
                typeof(IconToggelButton), new PropertyMetadata(Visibility.Collapsed));

        #region ForeTip
        public string ForeToolTip {
            get { return (string)GetValue(ForeToolTipProperty); }
            set { SetValue(ForeToolTipProperty, value); }
        }
        public static readonly DependencyProperty ForeToolTipProperty =
            DependencyProperty.Register("ForeToolTip", typeof(string),
                typeof(IconToggelButton), new PropertyMetadata(null));
        #endregion

        #region BackTip
        public string BackToolTip {
            get { return (string)GetValue(BackToolTipProperty); }
            set { SetValue(BackToolTipProperty, value); }
        }
        public static readonly DependencyProperty BackToolTipProperty =
            DependencyProperty.Register("BackToolTip", typeof(string),
                typeof(IconToggelButton), new PropertyMetadata(null));
        #endregion

        #endregion



        #region
        public IconToggelButton()
        {
            this.DefaultStyleKey = typeof(IconToggelButton);
        }
        #endregion 
    }
}
