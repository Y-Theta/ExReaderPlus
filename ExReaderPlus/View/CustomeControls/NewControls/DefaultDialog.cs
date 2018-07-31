using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace ExReaderPlus.View {
    public sealed class DefaultDialog : ContentDialog {

        public Thickness PrimaryButtonMargin {
            get { return (Thickness)GetValue(PrimaryButtonMarginProperty); }
            set { SetValue(PrimaryButtonMarginProperty, value); }
        }
        public static readonly DependencyProperty PrimaryButtonMarginProperty =
            DependencyProperty.Register("PrimaryButtonMargin", typeof(Thickness),
                typeof(DefaultDialog), new PropertyMetadata(new Thickness(0)));

        public Thickness SecondaryButtonMargin {
            get { return (Thickness)GetValue(SecondaryButtonMarginProperty); }
            set { SetValue(SecondaryButtonMarginProperty, value); }
        }
        public static readonly DependencyProperty SecondaryButtonMarginProperty =
            DependencyProperty.Register("SecondaryButtonMargin", typeof(Thickness),
                typeof(DefaultDialog), new PropertyMetadata(new Thickness(0)));

        public Thickness CloseButtonMargin {
            get { return (Thickness)GetValue(CloseButtonMarginProperty); }
            set { SetValue(CloseButtonMarginProperty, value); }
        }
        public static readonly DependencyProperty CloseButtonMarginProperty =
            DependencyProperty.Register("CloseButtonMargin", typeof(Thickness),
                typeof(DefaultDialog), new PropertyMetadata(new Thickness(0)));

        protected override void OnApplyTemplate() {
            base.OnApplyTemplate();
        }

        private void DefaultDialog_SizeChanged(object sender, SizeChangedEventArgs e) {
            
        }

        public DefaultDialog() {
            DefaultStyleKey = typeof(DefaultDialog);
            SizeChanged += DefaultDialog_SizeChanged;
        }
    }
}
