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
using System.Reflection;
using Windows.UI.Core;

namespace ExReaderPlus.View{
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
        /// 前一次渲染的单词的位置
        /// </summary>
        private Range _lastRange;

        /// <summary>
        /// 内容文本，节省每次预渲染时的开销
        /// </summary>
        private string _contentString;
        public string ContentString {
            get => _contentString;
            set { _contentString = value; }
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

        protected override void OnPointerEntered(PointerRoutedEventArgs e) {
            base.OnPointerEntered(e);
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e) {
            base.OnPointerPressed(e);
            Range range;
            string str;
            GetPointedWord(e.GetCurrentPoint(this).Position, out range, out str);
            if (range is null)
                return;
            Prerender(range,str);
        }

        protected override void OnPointerMoved(PointerRoutedEventArgs e) {
            base.OnPointerMoved(e);
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
        /// 用计时器异步降低开销
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
        /// 更新字典 txt-txt
        /// </summary>
        private async void UpdateDic() {
            string s = "";
            ElementsLoc.Clear();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                SetContentFormat(new Action(() => {
                    Document.GetText(TextGetOptions.None, out s);
                    Document.Selection.StartPosition = 0;
                    Document.Selection.EndPosition = s.Length;
                    Document.Selection.CharacterFormat = Document.GetDefaultCharacterFormat();
                    Document.Selection.StartPosition = s.Length;
                }));
            });
            ContentString = s;
            MatchCollection mc = Regex.Matches(s, @"[a-zA-Z-]+");
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
        private void GetPointedWord(Point p, out Range value, out string str) {
            if (ContentString is null)
            {
                value = null;
                str = null;
                return;
            }
            var tr = Document.GetRangeFromPoint(p, PointOptions.ClientCoordinates);
            int offsf = tr.StartPosition, offsb = tr.StartPosition;
            while (offsf <= ContentString.Length - 1 && ((ContentString[offsf] >= 'a' && ContentString[offsf] <= 'z') || (ContentString[offsf] >= 'A' && ContentString[offsf] <= 'Z') || ContentString[offsf].Equals('-')))
                offsf++;
            while (offsb >= 0 && ((ContentString[offsb] >= 'a' && ContentString[offsb] <= 'z') || (ContentString[offsb] >= 'A' && ContentString[offsb] <= 'Z') || ContentString[offsb].Equals('-')))
                offsb--;

            string s = "";
            try { s = ContentString.Substring(offsb + 1, offsf - offsb - 1); }
            catch (Exception) { s = ""; }
            str = s;
            value = new Range(offsb + 1, offsf);
        }

        /// <summary>
        /// 渲染词库中的单词项
        /// </summary>
        private void RenderWord() {
            this.TextChanged -= RichTextBox_TextChanged;
            if (Keywords != null && Keywords.Count > 0)
            {
                foreach (var kw in Keywords)
                {
                    kw.OldFormat = Document.GetDefaultCharacterFormat();
                    //  List<Range> 
                }
            }
        }

        /// <summary>
        /// 预渲染选中单词，TextPrerender = true
        /// </summary>
        private void Prerender(Range range,string str) {
            if (TextPrerender )
            {
                if (Keywords != null && Keywords.Count > 0 && Keywords[0].Words.Contains(str))
                    return;
                if (_lastRange != null) {
                    SetDefaultformat(_lastRange);
                }
                SetContentFormat(new Action(() =>{
                    Document.Selection.StartPosition = range.Start;
                    Document.Selection.EndPosition = range.End;
                    Document.Selection.CharacterFormat.Weight = 500;
                    Document.Selection.CharacterFormat.Size = 8;
                    Document.Selection.CharacterFormat.Spacing = 4;
                }));
                _lastRange = new Range(range);
            }
            //Rect rect;
            //int hit;
            //selection.GetRect(PointOptions.None, out rect, out hit);
            //Debug.WriteLine(string.Format("{0}   {1}        {2}", rect.Width, rect.Height, hit));
        }

        /// <summary>
        /// 将块格式还原为默认
        /// </summary>
        /// <param name="range"></param>
        private void SetDefaultformat(Range range) {
            SetContentFormat(new Action(() =>{
                Document.Selection.StartPosition = range.Start;
                Document.Selection.EndPosition = range.End;
                Document.Selection.CharacterFormat = Document.GetDefaultCharacterFormat();
            }));
        }

        /// <summary>
        /// 将设置格式的操作封装以避免触发textchanged
        /// </summary>
        /// <param name="action">设置格式的操作</param>
        private void SetContentFormat(Action action) {
            this.TextChanged -= RichTextBox_TextChanged;
            action?.Invoke();
            this.TextChanged += RichTextBox_TextChanged;
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