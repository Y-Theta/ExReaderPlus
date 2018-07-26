using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Timers;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ExReaderPlus.Models;
using Windows.UI.Xaml.Input;
using Windows.UI.Text;
using Windows.UI.Core;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Xaml.Media;
using System.Threading.Tasks;
using System.Text;

namespace ExReaderPlus.View {
    /// <summary>
    /// 自定义富文本框，继承自系统富文本编辑框，在此之中处理文字
    /// 渲染等操作，通过事件将操作暴露
    /// </summary>
    public sealed class RichTextBox : RichEditBox {

        #region Properties

        /// <summary>
        /// 计时器，用于在文本变化后一定时间内刷新字典
        /// </summary>
        private Timer _refreshdic;

        private double _lastHeight;

        private bool _refrash = false;

        private bool _oldReadonly;

        private bool _allowSwitch = true;

        private bool _switchPage = false;

        private bool _fontchange = false;


        private double _viewPortHeight;
        public double ViewPortHeight {
            get => _viewPortHeight;
            set {
                _refrash = true;
                _viewPortHeight = value;
                OnWordChanged();
            }
        }

        /// <summary>
        /// 转换完成标志,若不为0表示文本有变动
        /// </summary>
        private bool _transformComplete = true;
        public bool TransformComplete {
            get => _transformComplete;
            private set {
                _transformComplete = value;
                if (!_refreshdic.Enabled)
                    _refreshdic.Enabled = true;
            }
        }

        /// <summary>
        /// 单词字典
        /// </summary>
        private Dictionary<string, List<Rect>> _elementsLoc;
        public Dictionary<string, List<Rect>> ElementsLoc {
            get => _elementsLoc;
            private set { _elementsLoc = value; }
        }

        /// <summary>
        /// 内容文本，节省每次预渲染时的开销
        /// </summary>
        private string _contentString = null;
        public string ContentString {
            get => _contentString;
            set {
                _contentString = value;
                PassageLoaded?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 文章页表
        /// </summary>
        private List<Range> _pages;
        public List<Range> Pages {
            get => _pages;
            private set => _pages = value;
        }

        /// <summary>
        /// 文章当前页
        /// </summary>
        private int _temppage = 0;
        public int TempPage {
            get => _temppage;
            private set {
                if (value >= 0 && value < SumPage)
                    _temppage = value;
            }
        }

        private string _temppagecontent;
        public string Temppagecontent {
            get => _temppagecontent;
            set => _temppagecontent = value;
        }

        /// <summary>
        /// 总页数
        /// </summary>
        private int _sumPage;
        public int SumPage {
            get => _sumPage;
            private set => _sumPage = value;
        }

        /// <summary>
        /// 暴露IsReadOnly,增加修改回调
        /// </summary>
        public event EventHandler IsReadOnlyChanged;
        public new bool IsReadOnly {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set {
                SetValue(IsReadOnlyProperty, value);
                IsReadOnlyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 暴露字体
        /// </summary>
        public new double FontSize {
            get { return (double)GetValue(FontSizeProperty); }
            set {
                if (!_fontchange)
                {
                    _fontchange = true;
                    SetValue(FontSizeProperty, value);
                }
            }
        }

        /// <summary>
        /// 文字预渲染，默认关闭
        /// </summary>
        public bool TextPrerender {
            get { return (bool)GetValue(TextPrerenderProperty); }
            set { SetValue(TextPrerenderProperty, value); }
        }
        public static readonly DependencyProperty TextPrerenderProperty =
            DependencyProperty.Register("TextPrerender", typeof(bool),
                typeof(RichTextBox), new PropertyMetadata(false));
        #endregion

        #region Events

        /// <summary>
        /// 文本载入通知
        /// </summary>
        public event EventHandler PassageLoaded;

        /// <summary>
        /// 字典更新结束
        /// </summary>
        public event EventHandler ElementSorted;

        /// <summary>
        /// 渲染开始
        /// </summary>
        public event EventHandler RenderBegin;
        #endregion


        #region overrides
        private void RichTextBox_Paste(object sender, TextControlPasteEventArgs e) {
            OnWordChanged();
            _refrash = true;
        }

        private void RichTextBox_TextChanged(object sender, RoutedEventArgs e) {
            if (_contentString != null)
            {
                Document.GetText(TextGetOptions.None, out string newstr);
                _contentString = _contentString.Replace(Temppagecontent, newstr);
                if (!_switchPage)
                    OnWordChanged();
            }
            _refrash = true;
        }
        #endregion


        #region PrivateMotheds
        /// <summary>
        /// 初始化计时器,为了避免文字变化时同步处理,
        /// 用计时器异步降低开销
        /// </summary>
        private void InitTimer() {
            _refreshdic = new Timer { Interval = 2000 };
            _refreshdic.Elapsed += _refreshdic_Elapsed;
        }

        private async void _refreshdic_Elapsed(object sender, ElapsedEventArgs e) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                UpdateDic(Pages[TempPage]);
                _transformComplete = true;
                _refreshdic.Enabled = false;
            });
        }

        /// <summary>
        /// 文本可编辑状态改变
        /// </summary>
        private void RichTextBox_IsReadOnlyChanged(object sender, EventArgs e) {
            IsEnabled = false;
            if (_contentString != null)
                TransformComplete = false;
        }

        /// <summary>
        /// 文章载入后
        /// </summary>
        private void RichTextBox_PassageLoaded(object sender, EventArgs e) {
            AddBlankFirst(ref _contentString);
            OnWordChanged();
        }

        /// <summary>
        /// 文本分页
        /// </summary>
        private void SortPages(int firstpage) {
            int tempos = 0;
            SumPage = 0;
            Pages.Clear();
            for (int i = 0; i < ContentString.Length; i++)
            {
                int offset = 0;
                if(tempos + firstpage == ContentString.Length-1)
                    Pages.Add(new Range(tempos, firstpage));
                while (tempos + firstpage - offset < ContentString.Length && !ContentString[tempos + firstpage - offset].Equals(' ') && !ContentString[tempos + firstpage - offset].Equals('\n'))
                    offset++;
                if (tempos + firstpage - offset < ContentString.Length)
                    Pages.Add(new Range(tempos, firstpage - offset));
                else
                {
                    Pages.Add(new Range(tempos, ContentString.Length - tempos - 1));
                    SumPage++;
                    return;
                }
                SumPage++;
                tempos = tempos + firstpage - offset + 1;
            }
        }

        /// <summary>
        /// 文字改变时重新分页
        /// </summary>
        private void OnWordChanged() {
            IsEnabled = false;
            _allowSwitch = false;
            if (_contentString != null)
                SetContentFormat(() =>
                {
                    Document.SetText(TextSetOptions.None, _contentString);
                    ITextRange ran = Document.GetRangeFromPoint(new Point(ActualWidth, ViewPortHeight - Margin.Bottom - Padding.Bottom - 1.2 * FontSize), PointOptions.ClientCoordinates);
                    SortPages(ran.EndPosition);
                    SwitchPage();
                });
        }

        /// <summary>
        /// 换页
        /// </summary>
        private void SwitchPage() {
            RenderBegin?.Invoke(this, EventArgs.Empty);
            _allowSwitch = false;
            _refrash = true;
            if (_contentString != null)
                SetIgnoreReadonly(() =>
                {
                    _switchPage = true;
                    IsEnabled = false;
                    string str = _contentString.Substring(Pages[TempPage].Start, Pages[TempPage].End);
                    _temppagecontent = str;
                    Document.SetText(TextSetOptions.None, str);
                    TransformComplete = false;
                });
        }

        /// <summary>
        /// 更新字典
        /// </summary>
        private void UpdateDic(Range range) {
            if (range != null)
            {
                if (_refrash)
                    SetContentFormat(() =>
                    {
                        ElementsLoc.Clear();
                        _refrash = false;
                        Document.GetText(TextGetOptions.None, out string str);
                        MatchCollection mc = Regex.Matches(str, @"[a-zA-Z-]+");
                        foreach (Match m in mc)
                        {
                            Document.Selection.StartPosition = m.Index;
                            Document.Selection.EndPosition = m.Index + m.Length;
                            Document.Selection.GetRect(PointOptions.ClientCoordinates, out Rect outrect, out int hit);
                            AddtoLocDic(m.Value, outrect);
                        }
                        Document.Selection.StartPosition = Document.Selection.EndPosition = 0;
                        //foreach (var pk in ElementsLoc)
                        //{
                        //    Debug.WriteLine(pk.Key + "        " + PrintDicItem(pk.Value));
                        //}
                    });
                ElementSorted?.Invoke(this, EventArgs.Empty);
                //Switch
                _fontchange = false;
                _allowSwitch = true;
                _switchPage = false;
            }
        }

        private string AddBlankFirst(ref string str) {
            str = "    " + str;
            return str.Replace("\n", "\n    ");
        }

        /// <summary>
        /// 由于文章中会有重复的单词，所以用这种方式添加到字典
        /// </summary>
        private void AddtoLocDic(string key, Rect value) {
            if (_lastHeight > 0 && value.Height > _lastHeight * 1.5)
            {
                Rect top = new Rect(value.Right, value.Top, ActualWidth - value.Right - Padding.Right - Margin.Right, value.Height / 2);
                Rect tile = new Rect(0 + Margin.Left + Padding.Right, value.Top + value.Height / 2, value.Left - Margin.Right - Padding.Right, value.Height / 2);
                AddtoLocDic(key, top);
                AddtoLocDic(key, tile);
            }
            else
            {
                _lastHeight = value.Height;
                if (ElementsLoc.ContainsKey(key))
                {
                    ElementsLoc[key].Add(value);
                }
                else
                {
                    ElementsLoc.Add(key, new List<Rect>() { value });
                }
            }
        }

        /// <summary>
        /// 将设置格式的操作封装以避免触发textchanged
        /// </summary>
        /// <param name="action">设置格式的操作</param>
        private void SetContentFormat(Action action) {
            TextChanged -= RichTextBox_TextChanged;
            SetIgnoreReadonly(action);
            TextChanged += RichTextBox_TextChanged;
        }

        private void SetIgnoreReadonly(Action action) {
            IsReadOnlyChanged -= RichTextBox_IsReadOnlyChanged;
            if (IsReadOnly)
            {
                _oldReadonly = true;
                IsReadOnly = false;
            }
            action?.Invoke();
            if (_oldReadonly)
            {
                _oldReadonly = false;
                IsReadOnly = true;
            }
            IsReadOnlyChanged += RichTextBox_IsReadOnlyChanged;
        }

        private void RichTextBox_Loaded(object sender, RoutedEventArgs e) {
            SetDefaultFormat();
        }

        private void SetDefaultFormat() {
            FontSize = OverallViewSettings.Instence.RichTextBoxSize;

            ITextCharacterFormat defaultformat = Document.GetDefaultCharacterFormat();
            defaultformat.ForegroundColor = OverallViewSettings.Instence.RichTextBoxFg;
            defaultformat.BackgroundColor = OverallViewSettings.Instence.RichTextBoxBg;
            defaultformat.Weight = OverallViewSettings.Instence.RichTextBoxWeight;

            ITextParagraphFormat defaultformatp = Document.GetDefaultParagraphFormat();
            defaultformatp.SetLineSpacing(LineSpacingRule.AtLeast, OverallViewSettings.Instence.RichTextBoxSpace);

            Document.SetDefaultCharacterFormat(defaultformat);
            Document.SetDefaultParagraphFormat(defaultformatp);
        }

        private void Updatalayout() {

        }

        private string PrintDicItem(List<Rect> ranges) {
            throw new NotImplementedException();
            string s = "";
            foreach (var r in ranges)
            {
                s += "    " + r.ToString();
            }
            return s.ToString();
        }
        #endregion


        #region InterfaceFun
        public void FreshLayout() {
            _refrash = false;
            OnWordChanged();
        }

        public void GotoPage(int index) {
            if (_allowSwitch)
            {
                TempPage = index;
                OnWordChanged();
            }
        }

        public void PageUp() {
            if (_allowSwitch)
            {
                TempPage += 1;
                OnWordChanged();
            }
        }

        public void PageDown() {
            if (_allowSwitch)
            {
                TempPage -= 1;
                SwitchPage();
            }
        }
        #endregion


        #region Constructor
        public RichTextBox() {
            InitTimer();
            ElementsLoc = new Dictionary<string, List<Rect>>();
            Pages = new List<Range>();
            PassageLoaded += RichTextBox_PassageLoaded;
            IsReadOnlyChanged += RichTextBox_IsReadOnlyChanged;
            Loaded += RichTextBox_Loaded;
            TextChanged += RichTextBox_TextChanged;
            Paste += RichTextBox_Paste;
            DefaultStyleKey = typeof(RichTextBox);
        }
        #endregion
    }
}