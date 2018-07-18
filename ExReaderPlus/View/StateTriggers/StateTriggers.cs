using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ExReaderPlus.View.StateTriggers {
    public class BoolTrigger : StateTriggerBase {
        public bool ReferenceValue {
            get { return (bool)GetValue(ReferenceValueProperty); }
            set { SetValue(ReferenceValueProperty, value); }
        }
        public static readonly DependencyProperty ReferenceValueProperty =
            DependencyProperty.Register("ReferenceValue", typeof(bool), typeof(BoolTrigger), new PropertyMetadata(false));

        public bool BindValue {
            get { return (bool)GetValue(BindValueProperty); }
            set { SetValue(BindValueProperty, value); }
        }
        public static readonly DependencyProperty BindValueProperty =
            DependencyProperty.Register("BindValue", typeof(bool), typeof(BoolTrigger), new PropertyMetadata(false, new PropertyChangedCallback(OnBindValueChanged)));
        private static void OnBindValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            BoolTrigger b = (BoolTrigger)d;
            b.SetActive((bool)e.NewValue.Equals(b.ReferenceValue));
        }
    }


    public class NullValueTrigger : StateTriggerBase {

    }
}
