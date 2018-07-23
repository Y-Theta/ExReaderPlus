using ExReaderPlus.Models;
using ExReaderPlus.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace ExReaderPlus.View {
    public sealed partial class RichWordView : UserControl {
        #region Properties
        public EssayPageViewModel viewModel;

        private Dictionary<string,List<Control>> _controlDic;
        public Dictionary<string, List<Control>> ControlDic {
            get => _controlDic;
            set => _controlDic = value;
        }

        /// <summary>
        /// 获取或设置是否渲染文字
        /// </summary>
        public bool RenderText {
            get { return (bool)GetValue(RenderTextProperty); }
            set { SetValue(RenderTextProperty, value); }
        }
        public static readonly DependencyProperty RenderTextProperty =
            DependencyProperty.Register("RenderText", typeof(bool),
                typeof(RichWordView), new PropertyMetadata(false));


        /// <summary>
        /// 关键字，需要渲染的关键字组
        /// </summary>
        public List<Rendergroup> Keywords {
            get { return (List<Rendergroup>)GetValue(KeywordsProperty); }
            set { SetValue(KeywordsProperty, value); }
        }
        public static readonly DependencyProperty KeywordsProperty =
            DependencyProperty.Register("Keywords", typeof(List<Rendergroup>),
                typeof(RichTextBox), new PropertyMetadata(null));
        #endregion

        public RichWordView() {
            ControlDic = new Dictionary<string, List<Control>>();
            InitializeComponent();
            Loaded += RIchWordView_Loaded;
            Unloaded += RIchWordView_Unloaded;
        }

        private void RIchWordView_Unloaded(object sender, RoutedEventArgs e) {
            viewModel.PassageLoaded -= EssayPage_PassageLoaded;

        }

        private void RIchWordView_Loaded(object sender, RoutedEventArgs e) {
            viewModel = (EssayPageViewModel)DataContext;
            viewModel.PassageLoaded += EssayPage_PassageLoaded;
            TextView.ElementSorted += TextView_ElementSorted; ;
        
        }

        /// <summary>
        /// 富文本框的字典构建完毕,转换到阅读模式
        /// </summary>
        private void TextView_ElementSorted(object sender, EventArgs e) {
            RichTextBox rtb = sender as RichTextBox;
            ControlDic.Clear();
            foreach (var kp in rtb.ElementsLoc) {
                foreach (var loc in kp.Value) {
                    Button rect = new Button
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = new SolidColorBrush(Color.FromArgb(180, 0, 0, 0)),
                        Style = (Style)Application.Current.Resources["BoundButtonStyle"],
                        Margin = new Thickness(loc.Left - 2, loc.Top, 0, 0),
                        Width = loc.Width + 4,
                        Height = loc.Height + 2,
                        Name = kp.Key
                    };
                    AddtoControlDic(kp.Key, rect);
                    RenderLayer.Children.Add(rect);
                }
            }
        }

        private void AddtoControlDic(string key, Control value) {
            if (ControlDic.ContainsKey(key))
            {
                ControlDic[key].Add(value);
            }
            else
            {
                ControlDic.Add(key, new List<Control>() { value });
            }
        }


        private async void EssayPage_PassageLoaded(object sender, EventArgs e) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TextView.Document.SetText(TextSetOptions.None, viewModel.TempPassage.Content);
            });
        }

        public void SetText(TextSetOptions opt, string str) {
            TextView.Document.SetText(opt, str);
        }

        private void Enable(object sender, RoutedEventArgs e) {
            if (TextView.IsReadOnly)
                TextView.IsReadOnly = false;
            else
                TextView.IsReadOnly = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            List<Control> ss = ControlDic["have"];
            foreach (var con in ss) {
                (con as Button).Background = new SolidColorBrush(Colors.Cyan);
            }
        }
    }
}
