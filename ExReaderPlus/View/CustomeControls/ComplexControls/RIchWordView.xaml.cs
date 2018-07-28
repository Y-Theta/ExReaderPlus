using ExReaderPlus.Manage;
using ExReaderPlus.Models;
using ExReaderPlus.Tile;
using ExReaderPlus.ViewModels;
using ExReaderPlus.WordsManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private Rect _controlbararea;

        private Rect _nextpagearea;

        private Rect _lastpagearea;

        private Rect _wordlistarea;

        private Point _rectpoint;

        private Point _oripoint;

        private bool _controlbarmove;


        private Dictionary<string, List<Control>> _controlDic;
        public Dictionary<string, List<Control>> ControlDic {
            get => _controlDic;
            set => _controlDic = value;
        }

        /// <summary>
        /// 目标词列表裁剪矩形
        /// </summary>
        public Rect WordPanelRect {
            get { return (Rect)GetValue(WordPanelRectProperty); }
            set { SetValue(WordPanelRectProperty, value); }
        }
        public static readonly DependencyProperty WordPanelRectProperty =
            DependencyProperty.Register("WordPanelRect", typeof(Rect), 
                typeof(RichWordView), new PropertyMetadata(null));
        #endregion

        #region Methods
        #region Overrides
        protected override void OnTapped(TappedRoutedEventArgs e) {
            base.OnTapped(e);
            if (_controlbararea.Contains(e.GetPosition(this)) && TextView.IsReadOnly)
                ControlLayer.Visibility = ControlLayer.Visibility.Equals(Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            else if (_lastpagearea.Contains(e.GetPosition(this)) && TextView.IsReadOnly)
                TextView.PageDown();
            else if (_nextpagearea.Contains(e.GetPosition(this)) && TextView.IsReadOnly) 
                TextView.PageUp();
            else if(_wordlistarea.Contains(e.GetPosition(this)) && TextView.IsReadOnly)
                WordPanelSwitch();
        }

        #endregion

        #region Eventhandel
        private void RichWordView_Unloaded(object sender, RoutedEventArgs e) {
            _viewModel.PassageLoaded -= EssayPage_PassageLoaded;
            _viewModel.ControlCommand -= _viewModel_ControlCommand;
            _viewModel.WordStateChanged -= _viewModel_WordStateChanged;
            _viewModel.ShownStateChanged -= _viewModel_ShownStateChanged;

        }

        private void RichWordView_Loaded(object sender, RoutedEventArgs e) {
            _viewModel = (EssayPageViewModel)DataContext;
            _viewModel.PassageLoaded += EssayPage_PassageLoaded;
            _viewModel.ControlCommand += _viewModel_ControlCommand;
            _viewModel.WordStateChanged += _viewModel_WordStateChanged;
            _viewModel.ShownStateChanged += _viewModel_ShownStateChanged;
            TextView.ElementSorted += TextView_ElementSorted;
            TextView.RenderBegin += TextView_RenderBegin;
            ControlLayer.PointerEntered += GridBg_PointerEntered;
            ControlLayer.PointerExited += ControlLayer_PointerExited;
        }

        private void _viewModel_ShownStateChanged(object sender) {
            _viewModel.LoadKeywordList(Avb_RemCommandAction, PointedItem, ReSetPointedItem);
        }

        private void _viewModel_WordStateChanged(object sender) {
            ActionVocabulary avb = sender as ActionVocabulary;
            ChangControlColor(avb.Word, avb.YesorNo == 0 ? _viewModel.NotLearnBg : _viewModel.LearnedBg);
        }

        /// <summary>
        /// 按钮命令回调
        /// </summary>
        private void _viewModel_ControlCommand(object sender, CommandArgs args) {
            switch (args.parameter)
            {
                case "TurnPageNext": TextView.PageUp(); break;
                case "TurnPageBack": TextView.PageDown(); break;
                case "SizeTextLarge": TextView.FontSize += 0.5; break;
                case "SizeTextLittle": TextView.FontSize -= 0.5; break;
                case "OpenWordList": WordPanelSwitch(); break;
                case "Pin": TileService.PinTile();break;
                case "ChangeMode":
                    if (_viewModel.TempPassage != null)
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
                - TextScroll.Margin.Bottom - TextScroll.Margin.Top - TextScroll.Padding.Bottom - TextScroll.Padding.Top;
            ArrangeRect();
        }

        private void ArrangeRect() {
            _controlbararea = new Rect(TextScroll.ActualWidth / 3, TextScroll.ActualHeight / 3, TextScroll.ActualWidth / 3, TextScroll.ActualHeight / 3);
            _nextpagearea = new Rect(TextScroll.ActualWidth * 5 / 6, 0, TextScroll.ActualWidth / 6, TextScroll.ActualHeight);
            _lastpagearea = new Rect(0, 0, TextScroll.ActualWidth / 6, TextScroll.ActualHeight);
            _wordlistarea = new Rect(TextScroll.ActualWidth / 3, 0, TextScroll.ActualWidth / 3, TextScroll.ActualHeight / 3);
            WordPanelRect = new Rect(0, 0, 280, ActualHeight);
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
            _viewModel.ClearKeyWordHashSet();
            ControlDic.Clear();
            RenderLayer.Children.Clear();
            RenderLayer.UpdateLayout();
            if (rtb.ElementsLoc != null && rtb.ElementsLoc.Count > 0)
                foreach (var kp in rtb.ElementsLoc)
                    foreach (var loc in kp.Value)
                    {
                        HitHolder rect = new HitHolder
                        {
                            PointBrush = _viewModel.NormalBg,
                            Margin = new Thickness(loc.Left, loc.Top + 2, 0, 0),
                            Width = loc.Width,
                            Height = loc.Height - 2,
                            Name = kp.Key
                        };
                        if (WordBook.GetDicNow().Wordlist.ContainsKey(kp.Key))
                        {
                            Vocabulary vc = WordBook.GetDicNow().Wordlist[kp.Key];
                            if (vc.YesorNo == 0)
                            {
                                rect.Background = _viewModel.NotLearnBg;
                                _viewModel.KeyWordNotLearn.Add(kp.Key);
                            }
                            else {
                                rect.Background = _viewModel.LearnedBg;
                                _viewModel.KeyWordLearn.Add(kp.Key);
                            }
                        }
                        rect.PointerEntered += Rect_PointerEntered;
                        AddtoControlDic(kp.Key, rect);
                        RenderLayer.Children.Add(rect);
                    }
            RenderLayer.UpdateLayout();
            TextView.IsEnabled = true;
            if (TextView.IsReadOnly)
            {
                RenderLayer.Visibility = Visibility.Visible;
                if (WordPanel.Visibility.Equals(Visibility.Visible))
                    _viewModel.LoadKeywordList(Avb_RemCommandAction, PointedItem, ReSetPointedItem);
            }
        }

        private void PointedItem(object sender) {
            ActionVocabulary s = (ActionVocabulary)sender;
            ChangControlColor(s.Word, _viewModel.NormalBg);
        }

        private void ReSetPointedItem(object sender) {
            ActionVocabulary s = (ActionVocabulary)sender;
            foreach (var hithold in ControlDic[s.Word])
                if (s.YesorNo == 0)
                    (hithold as HitHolder).Background = _viewModel.NotLearnBg;
                else
                    (hithold as HitHolder).Background = _viewModel.LearnedBg;
        }

        private void Avb_RemCommandAction(object sender, CommandArgs args) {
            ActionVocabulary s = (ActionVocabulary)sender;
            ChangControlColor(s.Word, _viewModel.LearnedBg);
            if (s.YesorNo == 0)
            {
                _viewModel.KeyWordNotLearn.Remove(s.Word);
                _viewModel.KeyWordLearn.Add(s.Word);
                _viewModel.UpdateKeywordList(s);
            }
            else
            {
                _viewModel.KeyWordLearn.Remove(s.Word);
                _viewModel.KeyWordNotLearn.Add(s.Word);
                _viewModel.UpdateKeywordList(s);
            }
        }

        private void Rect_PointerEntered(object sender, PointerRoutedEventArgs e) {
            var sb = sender as HitHolder;
            var v1 = fileDatabaseManage.instance.SearchVocabulary(sb.Name.ToLower());
            if (v1 != null)
                sb.Tooltip = v1.Translation.Replace(@"\n", "\n");
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

        #endregion

        #region PrivateMethods
        public void SetText(string str) {
            TextView.ContentString = str;
        }

        private void ChangControlColor(string str, Brush color) {
            foreach (var hithold in ControlDic[str])
                (hithold as HitHolder).Background = color;
        }

        private void WordPanelSwitch() {
            if (WordPanel.Visibility.Equals(Visibility.Visible))
            {
                VisualStateManager.GoToState(this, "WordPanelCollapsed", true);
                _viewModel.SetStateBarButtonFg(Color.FromArgb(255, 8, 8, 8));
            }
            else
            {
                VisualStateManager.GoToState(this, "WordPanelShow", true);
                _viewModel.SetStateBarButtonFg(Color.FromArgb(255, 225, 225, 225));
            }
        }

        private void WordPanelState_CurrentStateChanged(object sender, VisualStateChangedEventArgs e) {
            TextView.FreshLayout();
            ArrangeRect();
        }

        private void InitVisualStates() {
            VisualStateManager.GoToState(this, "WordPanelCollapsed", false);
        }

        private void AttachMethods() {
            SizeChanged += RichWordView_SizeChanged;
            Loaded += RichWordView_Loaded;
            Unloaded += RichWordView_Unloaded;
        }

        private void InitCollections() {
            ControlDic = new Dictionary<string, List<Control>>();
        }

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
            InitCollections();
            AttachMethods();
            InitializeComponent();
            InitVisualStates();
        }
        #endregion

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
//            FileManage.FileManage fileManage= new FileManage.FileManage();
//            fileManage.NewPage();
        }

    }
}
