using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ExReaderPlus.View {
    public sealed class IconViewItem : NavigationViewItem {

        #region Properties
        /// <summary>
        /// 是否使用圆形头像
        /// </summary>
        public bool Round {
            get { return (bool)GetValue(RoundProperty); }
            set { SetValue(RoundProperty, value); }
        }
        public static readonly DependencyProperty RoundProperty =
            DependencyProperty.Register("Round", typeof(bool), 
                typeof(IconViewItem), new PropertyMetadata(false));

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
        #endregion

        #region Methods
        #endregion

        #region Constructors
        public IconViewItem() {

            this.DefaultStyleKey = typeof(IconViewItem);
        }
        #endregion
    }

}
