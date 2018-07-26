using ExReaderPlus.ViewModels;
using ExReaderPlus.WordsManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ExReaderPlus.View {
    public sealed partial class RichWordView : UserControl {
        #region Properties
        private EssayPageViewModel _viewModel;

        private Timer _autohidetimer;

        private Rect _controlbartap;

        private Rect _nextpagearea;

        private Rect _lastpagearea;

        private Point _rectpoint;

        private Point _oripoint;

        private bool _controlbarmove;


        private Dictionary<string, List<Control>> _controlDic;
        public Dictionary<string, List<Control>> ControlDic {
            get => _controlDic;
            set => _controlDic = value;
        }

        /// <summary>
        /// 关键字，需要渲染的关键字组
        /// </summary>
        public HashSet<string> KeyWords {
            get { return (HashSet<string>)GetValue(KeyWordsProperty); }
            set { SetValue(KeyWordsProperty, value); }
        }
        public static readonly DependencyProperty KeyWordsProperty =
            DependencyProperty.Register("KeyWords", typeof(HashSet<string>), 
                typeof(RichWordView), new PropertyMetadata(null));

        #endregion

        #region Methods
        #region Overrides
        protected override void OnTapped(TappedRoutedEventArgs e) {
            base.OnTapped(e);
            if (_controlbartap.Contains(e.GetPosition(this)) && TextView.IsReadOnly)
            {
                ControlLayer.Visibility = ControlLayer.Visibility.Equals(Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (_lastpagearea.Contains(e.GetPosition(this)) && TextView.IsReadOnly)
                TextView.PageDown();
            else if (_nextpagearea.Contains(e.GetPosition(this)) && TextView.IsReadOnly) 
                TextView.PageUp();

        }

        #endregion

        #region Eventhandel
        private void RichWordView_Unloaded(object sender, RoutedEventArgs e) {
            _viewModel.PassageLoaded -= EssayPage_PassageLoaded;
            _viewModel.ControlCommand -= _viewModel_ControlCommand;
        }

        private void RichWordView_Loaded(object sender, RoutedEventArgs e) {
            _viewModel = (EssayPageViewModel)DataContext;
            _viewModel.PassageLoaded += EssayPage_PassageLoaded;
            _viewModel.ControlCommand += _viewModel_ControlCommand;
            TextView.ElementSorted += TextView_ElementSorted;
            TextView.RenderBegin += TextView_RenderBegin;
            ControlLayer.PointerEntered += GridBg_PointerEntered;
            ControlLayer.PointerExited += ControlLayer_PointerExited;
        }

        /// <summary>
        /// 按钮命令回调
        /// </summary>
        private void _viewModel_ControlCommand(object sender, CommandArgs args) {
            switch (args.command)
            {
                case "TurnPage":
                    if (args.parameter is null)
                        TextView.PageUp();
                    else
                        TextView.PageDown();
                    break;
                case "SizeText":
                    if (args.parameter is null)
                        TextView.FontSize += 0.5;
                    else
                        TextView.FontSize -= 0.5;
                    break;
                case "ChangeMode":
                    if (!TextView.IsReadOnly)
                        TextView.IsReadOnly = true;
                    else
                    {
                        TextView.IsReadOnly = false;
                        RenderLayer.Visibility = Visibility.Collapsed;
                    }
                    break;
            }
        }

        private void RichWordView_SizeChanged(object sender, SizeChangedEventArgs e) {
            TextView.ViewPortHeight = e.NewSize.Height
                - Rootgrid.Margin.Bottom - Rootgrid.Margin.Top - Rootgrid.Padding.Bottom - Rootgrid.Padding.Top;
            _controlbartap = new Rect(ActualWidth / 3, ActualHeight / 3, ActualWidth / 3, ActualHeight / 3);
            _nextpagearea = new Rect(ActualWidth * 5 / 6, 0, ActualWidth / 6, ActualHeight);
            _lastpagearea = new Rect(0, 0, ActualWidth / 6, ActualHeight);
        }

        /// <summary>
        /// 文本处理开始回调
        /// </summary>
        private void TextView_RenderBegin(object sender, EventArgs e) {
            RenderLayer.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 控制面板控制
        /// </summary>
        private void ControlLayer_PointerExited(object sender, PointerRoutedEventArgs e) {
            (sender as Grid).Opacity = 0.3;
        }

        private void GridBg_PointerEntered(object sender, PointerRoutedEventArgs e) {
            (sender as Grid).Opacity = 1;
        }

        /// <summary>
        /// 富文本框的字典构建完毕,转换到阅读模式
        /// </summary>
        private void TextView_ElementSorted(object sender, EventArgs e) {
            RichTextBox rtb = sender as RichTextBox;
            foreach (var cot in ControlDic)
                foreach (var loc in cot.Value)
                    (loc as HitHolder).PointerEntered -= Rect_PointerEntered;
            KeyWords.Clear();
            ControlDic.Clear();
            RenderLayer.Children.Clear();
            RenderLayer.UpdateLayout();
            if (rtb.ElementsLoc != null && rtb.ElementsLoc.Count > 0)
                foreach (var kp in rtb.ElementsLoc)
                    foreach (var loc in kp.Value)
                    {
                        HitHolder rect = new HitHolder
                        {
                            PointBrush = new SolidColorBrush(Color.FromArgb(48, 0, 120, 200)),
                            Margin = new Thickness(loc.Left, loc.Top + 2, 0, 0),
                            Width = loc.Width,
                            Height = loc.Height - 2,
                            Name = kp.Key
                        };
                        if (WordBook.CET6.Wordlist.ContainsKey(kp.Key))
                        {
                            rect.Background = new SolidColorBrush(Color.FromArgb(48, 0, 200, 120));
                            KeyWords.Add(kp.Key);
                        }
                        rect.PointerEntered += Rect_PointerEntered;
                        AddtoControlDic(kp.Key, rect);
                        RenderLayer.Children.Add(rect);
                    }
            RenderLayer.UpdateLayout();
            TextView.IsEnabled = true;
            if (TextView.IsReadOnly)
                RenderLayer.Visibility = Visibility.Visible;
        }

        private void Rect_PointerEntered(object sender, PointerRoutedEventArgs e) {
            //var control = (HitHolder)sender;
            //var t = new Translate();
            //t.Text = control.Name;
            //var s = t.GetResult();
            //control.Tooltip = s;
        }

        private void AddtoControlDic(string key, Control value) {
            if (ControlDic.ContainsKey(key))
                ControlDic[key].Add(value);
            else
                ControlDic.Add(key, new List<Control>() { value });
        }

        private async void EssayPage_PassageLoaded(object sender, EventArgs e) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TextView.ContentString = (sender as EssayPageViewModel).TempPassage.Content;
            });
        }

        public void SetText(string str) {
            TextView.ContentString = str;
        }

        #endregion

        #region PrivateMethods
        private void InitTimer() {
            _autohidetimer = new Timer { Interval = 3000 };
            _autohidetimer.Elapsed += _autohidetimer_Elapsed;
        }

        private async void _autohidetimer_Elapsed(object sender, ElapsedEventArgs e) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,()=>{

            });
        }

        #endregion

        #region draganddrop
        private void ControlLayer_DragStarting(UIElement sender, DragStartingEventArgs args) {
            _controlbarmove = true;
            _oripoint = args.GetPosition(this);
        }

        private void ControlLayer_DropCompleted(UIElement sender, DropCompletedEventArgs args) {
            _controlbarmove = false;
            Thickness tic = ControlLayer.Margin;
            tic.Bottom = tic.Bottom + _oripoint.Y - _rectpoint.Y < 0 ? 0 : tic.Bottom + _oripoint.Y - _rectpoint.Y;
            ControlLayer.Margin = tic;
        }

        private void ControlLayer_DragOver(object sender, DragEventArgs e) {
            e.DragUIOverride.IsGlyphVisible = false;
        }

        private void Rootgrid_DragLeave(object sender, DragEventArgs e) {
            if (_controlbarmove)
            {
                _rectpoint = e.GetPosition(this);
                if (_rectpoint.Y < 0)
                    _rectpoint = _oripoint;
            }
        }
        #endregion

        #endregion

        #region Constructor
        public RichWordView() {
            InitTimer();
            ControlDic = new Dictionary<string, List<Control>>();
            KeyWords = new HashSet<string>();
            SizeChanged += RichWordView_SizeChanged;
            Loaded += RichWordView_Loaded;
            Unloaded += RichWordView_Unloaded;
            InitializeComponent();
        }
        #endregion

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
//            FileManage.FileManage fileManage= new FileManage.FileManage();
//            fileManage.NewPage();
        }
    }
}
