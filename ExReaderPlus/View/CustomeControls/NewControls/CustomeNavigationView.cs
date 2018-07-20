using ExReaderPlus.View.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace ExReaderPlus.View {

    [ContentProperty(Name = "Content")]
    public sealed class CustomeNavigationView : Control {

        #region Properties
        private string _radiogroupname;

        #region PaneBg
        public Brush PaneBackground {
            get { return (Brush)GetValue(PaneBackgroundProperty); }
            set { SetValue(PaneBackgroundProperty, value); }
        }
        public static readonly DependencyProperty PaneBackgroundProperty =
            DependencyProperty.Register("PaneBackground", typeof(Brush),
                typeof(CustomeNavigationView), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 255, 255, 255))));
        #endregion

        #region IsPaneOpen
        /// <summary>
        /// 打开listview
        /// </summary>
        public bool IsPaneOpen {
            get { return (bool)GetValue(IsPaneOpenProperty); }
            set { SetValue(IsPaneOpenProperty, value); }
        }
        public static readonly DependencyProperty IsPaneOpenProperty =
            DependencyProperty.Register("IsPaneOpen", typeof(bool),
                typeof(CustomeNavigationView), new PropertyMetadata(false));
        #endregion

        #region Content
        /// <summary>
        /// 右侧内容区
        /// </summary>
        public UIElement Content {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElement),
                typeof(CustomeNavigationView), new PropertyMetadata(null));
        #endregion

        #region FunctionArea
        /// <summary>
        /// 项目菜单区
        /// </summary>
        public UIElement FunctionArea {
            get { return (UIElement)GetValue(FunctionAreaProperty); }
            set { SetValue(FunctionAreaProperty, value); }
        }
        public static readonly DependencyProperty FunctionAreaProperty =
            DependencyProperty.Register("FunctionArea", typeof(UIElement),
                typeof(CustomeNavigationView), new PropertyMetadata(null));
        #endregion

        #region PanelWidth

        #endregion

        //SizeProperties

        #region ClipOffset
        /// <summary>
        /// 使用Clip位移制作宽度动画
        /// </summary>
        public double ClipOffset {
            get { return (double)GetValue(ClipOffsetProperty); }
            set { SetValue(ClipOffsetProperty, value); }
        }
        public static readonly DependencyProperty ClipOffsetProperty =
            DependencyProperty.Register("ClipOffset", typeof(double), 
                typeof(CustomeNavigationView), new PropertyMetadata(0));
        #endregion

        #region PanelClip
        /// <summary>
        /// 裁剪矩形
        /// </summary>
        public Rect PaneClip {
            get { return (Rect)GetValue(PaneClipProperty); }
            set { SetValue(PaneClipProperty, value); }
        }
        private static readonly DependencyProperty PaneClipProperty =
            DependencyProperty.Register("PaneClip", typeof(Rect), 
                typeof(CustomeNavigationView), new PropertyMetadata(null));
        #endregion

        #region OpenWidth
        /// <summary>
        /// 展开宽度
        /// </summary>
        public double OpenWidth {
            get { return (double)GetValue(OpenWidthProperty); }
            set { SetValue(OpenWidthProperty, value); }
        }
        public static readonly DependencyProperty OpenWidthProperty =
            DependencyProperty.Register("OpenWidth", typeof(double),
                typeof(CustomeNavigationView), new PropertyMetadata(240.0));
        #endregion

        #region CollapseWidth
        /// <summary>
        /// 折叠宽度
        /// </summary>
        public double CollapseWidth {
            get { return (double)GetValue(CollapseWidthProperty); }
            set { SetValue(CollapseWidthProperty, value); }
        }
        public static readonly DependencyProperty CollapseWidthProperty =
            DependencyProperty.Register("CollapseWidth", typeof(double),
                typeof(CustomeNavigationView), new PropertyMetadata(48.0));
        #endregion

        #region OpenPaneCommand
        /// <summary>
        /// 打开pane命令
        /// </summary>
        public CommandBase OpenPaneCommand {
            get { return (CommandBase)GetValue(OpenPaneCommandProperty); }
            set { SetValue(OpenPaneCommandProperty, value); }
        }
        public static readonly DependencyProperty OpenPaneCommandProperty =
            DependencyProperty.Register("OpenPaneCommand", typeof(CommandBase),
                typeof(CustomeNavigationView), new PropertyMetadata(null));
        #endregion

        #region UserIcon
        public RadioButton UserIcon {
            get { return (RadioButton)GetValue(UserIconProperty); }
            set { SetValue(UserIconProperty, value); }
        }
        public static readonly DependencyProperty UserIconProperty =
            DependencyProperty.Register("UserIcon", typeof(RadioButton), 
                typeof(CustomeNavigationView), new PropertyMetadata(null, OnUserIconSet));

        private static void OnUserIconSet(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            (e.NewValue as RadioButton).GroupName = (d as CustomeNavigationView)._radiogroupname;
        }
        #endregion


        #region SettingButton
        public RadioButton SettingButton {
            get { return (RadioButton)GetValue(SettingButtonProperty); }
            set { SetValue(SettingButtonProperty, value); }
        }
        public static readonly DependencyProperty SettingButtonProperty =
            DependencyProperty.Register("SettingButton", typeof(RadioButton),
                typeof(CustomeNavigationView), new PropertyMetadata(null, OnSettingButtonSet));
        private static void OnSettingButtonSet(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            (e.NewValue as RadioButton).GroupName = (d as CustomeNavigationView)._radiogroupname;
        }
        #endregion

        #endregion


        #region Motheds
        private void InitCommands() {
            OpenPaneCommand = new CommandBase();
            OpenPaneCommand.Commandaction += OpenPaneCommand_Commandaction;
        }

        private void OpenPaneCommand_Commandaction(object parameter) {
            if (IsPaneOpen)
            {
                IsPaneOpen = false;
                VisualStateManager.GoToState(this, "CollapseMode_Collapse", true);
            }
            else
            {
                IsPaneOpen = true;
                VisualStateManager.GoToState(this, "CollapseMode_Open", true);
            }
        }

        private void CustomeNavigationView_Loaded(object sender, RoutedEventArgs e) {
            ClipOffset = OpenWidth - CollapseWidth;
            VisualStateManager.GoToState(this, "CollapseMode_Collapse", false);
        }

        private void CustomeNavigationView_SizeChanged(object sender, SizeChangedEventArgs e) {
            PaneClip = new Rect(0, 0, OpenWidth, e.NewSize.Height);
        }
        #endregion


        #region Contructors
        public CustomeNavigationView() {
            InitCommands();
            _radiogroupname = this.GetHashCode().ToString();
            this.Loaded += CustomeNavigationView_Loaded;
            this.SizeChanged += CustomeNavigationView_SizeChanged;
            this.DefaultStyleKey = typeof(CustomeNavigationView);
        }
        #endregion
    }
}
