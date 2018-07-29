using System;
using System.Collections.Generic;
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
        protected override void OnApplyTemplate() {
            base.OnApplyTemplate();
        }

       
        public DefaultDialog() {
            this.DefaultStyleKey = typeof(DefaultDialog);
        }
    }
}
