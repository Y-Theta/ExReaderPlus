using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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
    public sealed partial class DicItem : UserControl {
        #region Properties
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

        /// <summary>
        /// 单击命令
        /// </summary>
        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand),
                typeof(DicItem), new PropertyMetadata(null));

        /// <summary>
        /// 命令参数
        /// </summary>
        public object CommandPara {
            get { return (object)GetValue(CommandParaProperty); }
            set { SetValue(CommandParaProperty, value); }
        }
        public static readonly DependencyProperty CommandParaProperty =
            DependencyProperty.Register("CommandPara", typeof(object),
                typeof(DicItem), new PropertyMetadata(null));

        #endregion

        #region  Methods
        protected override void OnPointerPressed(PointerRoutedEventArgs e) {
            base.OnPointerPressed(e);
            VisualStateManager.GoToState(this, "Pressed", false);
            Command?.Execute(CommandPara);
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e) {
            base.OnPointerEntered(e);
            VisualStateManager.GoToState(this, "MouseOver", false);

        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e) {
            base.OnPointerReleased(e);
            VisualStateManager.GoToState(this, "MouseOver", false);

        }

        protected override void OnPointerExited(PointerRoutedEventArgs e) {
            base.OnPointerExited(e);
            VisualStateManager.GoToState(this, "Normal", false);

        }

        private void DicItem_SizeChanged(object sender, SizeChangedEventArgs e) {
            MessureWidth = e.NewSize.Width;
        }
        #endregion

        public DicItem() {
            InitializeComponent();
            SizeChanged += DicItem_SizeChanged;
        }
    }
}
