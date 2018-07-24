using ExReaderPlus.Baidu;
using ExReaderPlus.Models;
using ExReaderPlus.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace ExReaderPlus.View {
    public sealed partial class RichWordView : UserControl {
        #region Properties
        public EssayPageViewModel viewModel;

        private Dictionary<string, List<Control>> _controlDic;
        public Dictionary<string, List<Control>> ControlDic {
            get => _controlDic;
            set => _controlDic = value;
        }

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

        #region Methods
        private void RIchWordView_Unloaded(object sender, RoutedEventArgs e) {
            viewModel.PassageLoaded -= EssayPage_PassageLoaded;
        }

        private void RichWordView_SizeChanged(object sender, SizeChangedEventArgs e) {
            TextView.ViewPortHeight = e.NewSize.Height;
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
            RenderLayer.Children.Clear();
            RenderLayer.UpdateLayout();
            if (rtb.ElementsLoc != null && rtb.ElementsLoc.Count > 0)
                foreach (var kp in rtb.ElementsLoc)
                {
                    foreach (var loc in kp.Value)
                    {
                        HitHolder rect = new HitHolder
                        {
                            PointBrush = new SolidColorBrush(Color.FromArgb(200, 0, 120, 200)),
                            Margin = new Thickness(loc.Left, loc.Top + 2, 0, 0),
                            Width = loc.Width,
                            Height = loc.Height - 2,
                            Name = kp.Key
                        };
                        rect.PointerEntered += Rect_PointerEntered;
                        AddtoControlDic(kp.Key, rect);
                        RenderLayer.Children.Add(rect);
                    }
                }
            RenderLayer.UpdateLayout();
            TextView.IsEnabled = true;
            if (TextView.IsReadOnly)
                RenderLayer.Visibility = Visibility.Visible;
        }

        private void Rect_PointerEntered(object sender, PointerRoutedEventArgs e) {
            var control = (HitHolder)sender;
            var t = new Translate();
            t.Text = control.Name;
            var s = t.GetResult();
            control.Tooltip = s;
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
                SetText((sender as EssayPageViewModel).TempPassage.Content);
            });
        }

        public void SetText(string str) {
            TextView.ContentString = str;
        }

        private void Enable(object sender, RoutedEventArgs e) {
            if (!TextView.IsReadOnly)
            {
                TextView.IsReadOnly = true;
            }
            else
            {
                TextView.IsReadOnly = false;
                RenderLayer.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
           

        }
        #endregion

        public RichWordView() {
            ControlDic = new Dictionary<string, List<Control>>();
            InitializeComponent();
            SizeChanged += RichWordView_SizeChanged;
            Loaded += RIchWordView_Loaded;
            Unloaded += RIchWordView_Unloaded;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            TextView.PageDown();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            TextView.PageUp();

        }

        private void Button_Click_3(object sender, RoutedEventArgs e) {
            TextView.FontSize += 0.5;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e) {
            TextView.FontSize -= 0.5;
        }
    }
}
