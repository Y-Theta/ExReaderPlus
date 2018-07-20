using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

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
        #endregion

        #region Methods
        protected override void OnPointerPressed(PointerRoutedEventArgs e) {
            base.OnPointerPressed(e);
            if (Command != null) {
                if(Command.CanExecute(CommandParameter))
                    Command.Execute(CommandParameter);
            }
        }
        #endregion

        #region Constructors
        public IconViewItem() {
            this.DefaultStyleKey = typeof(IconViewItem);
        }
        #endregion
    }

}
