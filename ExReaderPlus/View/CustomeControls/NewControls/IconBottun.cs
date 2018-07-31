using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ExReaderPlus.View {
    public class IconBottun : Button {
        #region Properties
        public Brush PointBrush {
            get { return (Brush)GetValue(PointBrushProperty); }
            set { SetValue(PointBrushProperty, value); }
        }
        public static readonly DependencyProperty PointBrushProperty =
            DependencyProperty.Register("PointBrush", typeof(Brush),
                typeof(IconBottun), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public Brush PressBrush {
            get { return (Brush)GetValue(PressBrushProperty); }
            set { SetValue(PressBrushProperty, value); }
        }
        public static readonly DependencyProperty PressBrushProperty =
            DependencyProperty.Register("PressBrush", typeof(Brush),
                typeof(IconBottun), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public Brush PointMask {
            get { return (Brush)GetValue(PointMaskProperty); }
            set { SetValue(PointMaskProperty, value); }
        }
        public static readonly DependencyProperty PointMaskProperty =
            DependencyProperty.Register("PointMask", typeof(Brush),
                typeof(IconBottun), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public Brush PressMask {
            get { return (Brush)GetValue(PressMaskProperty); }
            set { SetValue(PressMaskProperty, value); }
        }
        public static readonly DependencyProperty PressMaskProperty =
            DependencyProperty.Register("PressMask", typeof(Brush),
                typeof(IconBottun), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public string Icon {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string),
                typeof(IconBottun), new PropertyMetadata(null));

        #region ForeTip
        public string ToolTip {
            get { return (string)GetValue(ToolTipProperty); }
            set { SetValue(ToolTipProperty, value); }
        }
        public static readonly DependencyProperty ToolTipProperty =
            DependencyProperty.Register("ToolTip", typeof(string),
                typeof(IconBottun), new PropertyMetadata(null));
        #endregion

        #endregion

        #region Methods
        #endregion

        #region Constructors
        public IconBottun() {
            DefaultStyleKey = typeof(IconBottun);
        }
        #endregion
    }

}
