using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ExReaderPlus.Models;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Text;
using Windows.UI;

namespace ExReaderPlus.CustomeControls.NewControls {
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

        /// <summary>
        /// 内容文本，节省每次预渲染时的开销
        /// </summary>
        private string _contentString;

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
        private Dictionary<string, List<Range>> _elementsLoc;
        public Dictionary<string, List<Range>> ElementsLoc {
            get => _elementsLoc;
            private set { _elementsLoc = value; }
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
                typeof(RichTextBox), new PropertyMetadata(null, KeywordsChanged));
        private static void KeywordsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {

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
        /// 字典更新结束
        /// </summary>
        public event EventHandler ElementSorted;

        /// <summary>
        /// 渲染结束
        /// </summary>
        public event EventHandler RenderComplete;
        #endregion


        #region overrides
        protected override void OnApplyTemplate() {
            base.OnApplyTemplate();
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e) {
            base.OnPointerPressed(e);
            var s = GetPointedWord(e.GetCurrentPoint(this).Position);
            Debug.WriteLine(s);
        }

        private void RichTextBox_Paste(object sender, TextControlPasteEventArgs e) {

        }

        private void RichTextBox_TextChanged(object sender, RoutedEventArgs e) {
            TransformComplete = false;
        }
        #endregion


        #region PrivateMotheds
        /// <summary>
        /// 初始化计时器,为了避免文字变化时同步处理,
        /// 用计时器降低开销
        /// </summary>
        private void InitTimer() {
            _refreshdic = new Timer { Interval = 3000 };
            _refreshdic.Elapsed += _refreshdic_Elapsed;
        }

        private void _refreshdic_Elapsed(object sender, ElapsedEventArgs e) {
            if (!_transformComplete)
            {
                UpdateDic();
                _transformComplete = true;
            }
            else
            {
                _refreshdic.Enabled = false;
                ElementSorted?.Invoke(this, null);
            }
        }

        /// <summary>
        /// 更新字典
        /// </summary>
        private void UpdateDic() {
            string s = "";
            ElementsLoc.Clear();
            this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Document.GetText(Windows.UI.Text.TextGetOptions.None, out s);
            }).AsTask().Wait();
            MatchCollection mc = Regex.Matches(s, @"[a-zA-Z]+");
            foreach (Match m in mc)
            {
                AddtoLocDic(m.Value, new Range(m.Index, m.Index + m.Length));
            }

            //foreach (var kv in ElementsLoc)
            //    Debug.WriteLine(String.Format("{0}   {1}", kv.Key, PrintDicItem(kv.Value)));
        }

        /// <summary>
        /// 由于文章中会有重复的单词，所以用这种方式添加到字典
        /// </summary>
        private void AddtoLocDic(string key, Range value) {
            if (ElementsLoc.ContainsKey(key))
            {
                ElementsLoc[key].Add(value);
            }
            else
            {
                ElementsLoc.Add(key, new List<Range>() { value });
            }
        }

        /// <summary>
        /// 获取选中位置的单词
        /// </summary>
        /// <param name="p">鼠标位置p</param>
        /// <returns>当前位置最近的单词</returns>
        private string GetPointedWord(Point p) {
            string s = "";
            var tr = Document.GetRangeFromPoint(p, PointOptions.ClientCoordinates);
            int offsf = tr.StartPosition, offsb = tr.StartPosition;
            Document.GetText(TextGetOptions.None, out s);
            while (offsf <= s.Length - 1 && ((s[offsf] >= 'a' && s[offsf] <= 'z') || (s[offsf] >= 'A' && s[offsf] <= 'Z') || s[offsf].Equals('-')))
                offsf++;
            while (offsb >= 0 && ((s[offsb] >= 'a' && s[offsb] <= 'z') || (s[offsb] >= 'A' && s[offsb] <= 'Z') || s[offsb].Equals('-')))
                offsb--;

            try { s = s.Substring(offsb + 1, offsf - offsb - 1); }
            catch (Exception) { s = ""; }

            return s;
        }

        /// <summary>
        /// 渲染词库中的单词项
        /// </summary>
        private void RenderWord() {

        }

        /// <summary>
        /// 预渲染选中单词，TextPrerender = true
        /// </summary>
        private void Prerender() {
            if (TextPrerender) {

            }
        }

        private string PrintDicItem(List<Range> ranges) {
            // throw new NotImplementedException();
            string s = "";
            foreach (var r in ranges)
            {
                s += "    " + r.ToString();
            }
            return s.ToString();
        }
        #endregion


        #region Constructor
        public RichTextBox() {
            InitTimer();
            ElementsLoc = new Dictionary<string, List<Range>>();
            this.TextChanged += RichTextBox_TextChanged;
            this.Paste += RichTextBox_Paste;
            this.DefaultStyleKey = typeof(RichTextBox);
        }
        #endregion
    }
}