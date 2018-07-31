using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class VocabularyItem : UserControl {
        public Brush MainForeground {
            get { return (Brush)GetValue(MainForegroundProperty); }
            set { SetValue(MainForegroundProperty, value); }
        }
        public static readonly DependencyProperty MainForegroundProperty =
            DependencyProperty.Register("MainForeground", typeof(Brush),
                typeof(VocabularyItem), new PropertyMetadata(null));

        /// <summary>
        /// 鼠标悬浮颜色
        /// </summary>
        public Brush PointBrush {
            get { return (Brush)GetValue(PointBrushProperty); }
            set { SetValue(PointBrushProperty, value); }
        }
        public static readonly DependencyProperty PointBrushProperty =
            DependencyProperty.Register("PointBrush", typeof(Brush),
                typeof(VocabularyItem), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// 鼠标点击颜色
        /// </summary>
        public Brush PressBrush {
            get { return (Brush)GetValue(PressBrushProperty); }
            set { SetValue(PressBrushProperty, value); }
        }
        public static readonly DependencyProperty PressBrushProperty =
            DependencyProperty.Register("PressBrush", typeof(Brush),
                typeof(VocabularyItem), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// 鼠标悬浮事件
        /// </summary>
        public HCHPointHandel OnPointEnter {
            get { return (HCHPointHandel)GetValue(OnPointEnterrProperty); }
            set { SetValue(OnPointEnterrProperty, value); }
        }
        public static readonly DependencyProperty OnPointEnterrProperty =
            DependencyProperty.Register("OnPointEnter", typeof(HCHPointHandel),
                typeof(VocabularyItem), new PropertyMetadata(null));

        public HCHPointHandel OnPointExit {
            get { return (HCHPointHandel)GetValue(OnPointExitProperty); }
            set { SetValue(OnPointExitProperty, value); }
        }
        public static readonly DependencyProperty OnPointExitProperty =
            DependencyProperty.Register("OnPointExit", typeof(HCHPointHandel),
                typeof(VocabularyItem), new PropertyMetadata(null));

        public Style ButtonStyle {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }
        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), 
                typeof(VocabularyItem), new PropertyMetadata(null));

        public Style PhoneticStyle {
            get { return (Style)GetValue(PhoneticStyleProperty); }
            set { SetValue(PhoneticStyleProperty, value); }
        }
        public static readonly DependencyProperty PhoneticStyleProperty =
            DependencyProperty.Register("PhoneticStyle", typeof(Style), 
                typeof(VocabularyItem), new PropertyMetadata(null));

        public Style TranslationStyle {
            get { return (Style)GetValue(TranslationStyleProperty); }
            set { SetValue(TranslationStyleProperty, value); }
        }
        public static readonly DependencyProperty TranslationStyleProperty =
            DependencyProperty.Register("TranslationStyle", typeof(Style), 
                typeof(VocabularyItem), new PropertyMetadata(null));

        public bool ReadOnly {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }
        public static readonly DependencyProperty ReadOnlyProperty =
            DependencyProperty.Register("ReadOnly", typeof(bool), 
                typeof(VocabularyItem), new PropertyMetadata(false));


        #region Methods
        private void VocabularyItem_Loaded(object sender, RoutedEventArgs e) {
            
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e) {
            base.OnPointerPressed(e);
            VisualStateManager.GoToState(this, "Pressed", false);
            e.Handled = true;
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e) {
            base.OnPointerReleased(e);
            VisualStateManager.GoToState(this, "PointOver", false);
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e) {
            base.OnPointerEntered(e);
            VisualStateManager.GoToState(this, "PointOver", false);
            OnPointEnter?.Invoke(DataContext);
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e) {
            base.OnPointerExited(e);
            VisualStateManager.GoToState(this, "Normal", false);
            OnPointExit?.Invoke(DataContext);
        }

        #endregion


        public VocabularyItem() {
            InitializeComponent();
            Loaded += VocabularyItem_Loaded;
        }

    }
}
