using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ExReaderPlus.View {
    public sealed class IconViewItem : NavigationViewItem {


        #region Properties
        public enum IconKind {
            Icon,
            Rect,
            Round
        }

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
        /// 点击命令
        /// </summary>
        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), 
                typeof(IconViewItem), new PropertyMetadata(null));

        /// <summary>
        /// 命令参数
        /// </summary>
        public object CommandParameter {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), 
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
