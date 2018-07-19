﻿using ExReaderPlus.View.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        //private GridLength _collapseGridLength;
        //public GridLength CollapseGridLength {
           
        //}

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

        #region ListItems
        /// <summary>
        /// 菜单项
        /// </summary>
        public IList<object> MenuItems {
            get { return (IList<object>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }
        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register("MenuItems", typeof(IList<object>), 
                typeof(CustomeNavigationView), new PropertyMetadata(0));
        #endregion

        #region Content
        public UIElement Content {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElement),
                typeof(CustomeNavigationView), new PropertyMetadata(null));
        #endregion

        #region PanelWidth
        public double PanelWidth {
            get { return (double)GetValue(PanelWidthProperty); }
            set { SetValue(PanelWidthProperty, value); }
        }
        public static readonly DependencyProperty PanelWidthProperty =
            DependencyProperty.Register("PanelWidth", typeof(double),
                typeof(CustomeNavigationView), new PropertyMetadata(48.0));
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
        public CommandBase OpenPaneCommand {
            get { return (CommandBase)GetValue(OpenPaneCommandProperty); }
            set { SetValue(OpenPaneCommandProperty, value); }
        }
        public static readonly DependencyProperty OpenPaneCommandProperty =
            DependencyProperty.Register("OpenPaneCommand", typeof(CommandBase),
                typeof(CustomeNavigationView), new PropertyMetadata(null));
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
        #endregion


        #region Contructors
        public CustomeNavigationView() {
            InitCommands();
            this.DefaultStyleKey = typeof(CustomeNavigationView);
        }
        #endregion
    }
}