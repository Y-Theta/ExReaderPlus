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

        private bool _refrash = false;

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
        private Dictionary<string, List<Rect>> _elementsLoc;
        public Dictionary<string, List<Rect>> ElementsLoc {
            get => _elementsLoc;
            private set { _elementsLoc = value; }
        }


        /// <summary>
        /// 暴露IsReadOnly,增加修改回调
        /// </summary>
        public event EventHandler IsReadOnlyChanged;
        public new bool IsReadOnly {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set {
                IsReadOnlyChanged?.Invoke(this, EventArgs.Empty);
                SetValue(IsReadOnlyProperty, value);
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
        /// 字典更新结束
        /// </summary>
        public event EventHandler ElementSorted;

        /// <summary>
        /// 渲染结束
        /// </summary>
        public event EventHandler RenderComplete;

        /// <summary>
        /// 
        /// </summary>
        public event WordSelectEventHandler WordSelect;
        #endregion


        #region overrides
        protected override void OnApplyTemplate() {
            base.OnApplyTemplate();
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e) {
            base.OnPointerPressed(e);
        }

        protected override void OnPointerMoved(PointerRoutedEventArgs e) {
            base.OnPointerMoved(e);
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e) {
            base.OnPointerExited(e);
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e) {
            base.OnPointerEntered(e);
        }

        private void RichTextBox_Paste(object sender, TextControlPasteEventArgs e) {
            _refrash = true;
        }

        private void RichTextBox_TextChanged(object sender, RoutedEventArgs e) {
            _refrash = true;
        }
        #endregion


        #region PrivateMotheds
        private void RichTextBox_IsReadOnlyChanged(object sender, EventArgs e) {
            UpdateDic();
        }

        /// <summary>
        /// 初始化计时器,为了避免文字变化时同步处理,
        /// 用计时器异步降低开销
        /// </summary>
        private void InitTimer() {
            _refreshdic = new Timer { Interval = 3000 };
        }

        /// <summary>
        /// 更新字典 txt-txt
        /// </summary>
        private void UpdateDic() {
            string s = "";
            ElementsLoc.Clear();
            if(!IsReadOnly && _refrash)
            SetContentFormat(() =>
            {
                IsEnabled = false;
                IsReadOnly = false;
                Document.GetText(TextGetOptions.None, out s);
                Document.Selection.StartPosition = 0;
                Document.Selection.EndPosition = s.Length;
                Document.Selection.CharacterFormat = Document.GetDefaultCharacterFormat();
                ContentString = s;
                MatchCollection mc = Regex.Matches(s, @"[a-zA-Z-]+");
                foreach (Match m in mc)
                {
                    Document.Selection.StartPosition = m.Index;
                    Document.Selection.EndPosition = m.Index + m.Length;
                    Document.Selection.GetRect(PointOptions.ClientCoordinates, out Rect outrect, out int hit);
                    AddtoLocDic(m.Value, outrect);
                }
                Document.Selection.StartPosition = Document.Selection.EndPosition = 0;
                foreach (var pk in ElementsLoc)
                {
                    Debug.WriteLine(pk.Key + "        " + PrintDicItem(pk.Value));
                }
                IsReadOnly = true;
                IsEnabled = true;
            });
            ElementSorted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 由于文章中会有重复的单词，所以用这种方式添加到字典
        /// </summary>
        private void AddtoLocDic(string key, Rect value) {
            if (ElementsLoc.ContainsKey(key))
            {
                ElementsLoc[key].Add(value);
            }
            else
            {
                ElementsLoc.Add(key, new List<Rect>() { value });
            }
        }

        /// <summary>
        /// 获取选中位置的单词
        /// </summary>
        /// <param name="p">鼠标位置p</param>
        /// <returns>当前位置最近的单词</returns>
        private void GetPointedWord(Point p, out Range value, out string str, out Rect area) {
            if (ContentString is null)
            {
                value = null;
                str = null;
                area = new Rect(0, 0, 0, 0);
                return;
            }

            var tr = Document.GetRangeFromPoint(p, PointOptions.ClientCoordinates);
            int offsf = tr.StartPosition, offsb = tr.StartPosition;

            if (_transformComplete)
            {
                while (offsf <= ContentString.Length - 1 && ((ContentString[offsf] >= 'a' && ContentString[offsf] <= 'z') || (ContentString[offsf] >= 'A' && ContentString[offsf] <= 'Z') || ContentString[offsf].Equals('-')))
                    offsf++;
                while (offsb > 0 && ((ContentString[offsb] >= 'a' && ContentString[offsb] <= 'z') || (ContentString[offsb] >= 'A' && ContentString[offsb] <= 'Z') || ContentString[offsb].Equals('-')))
                    offsb--;
            }

            Rect outrect;
            SetContentFormat(() =>
            {
                Document.Selection.StartPosition = offsb + 1;
                Document.Selection.EndPosition = offsf;
                Document.Selection.GetRect(PointOptions.ClientCoordinates, out outrect, out int hit);
                Document.Selection.StartPosition = offsf;
            });

            string s = "";
            try { s = ContentString.Substring(offsb + 1, offsf - offsb - 1); }
            catch (Exception) { s = ""; }
            str = s;
            value = new Range(offsb + 1, offsf);
            area = outrect;
        }

        /// <summary>
        /// 预渲染选中单词，TextPrerender = true
        /// </summary>
        private void Prerender(Point p) {
            GetPointedWord(p, out Range range, out string str, out Rect rect);
            if (range is null)
                return;
            WordSelect?.Invoke(this, new WordSelectArgs(rect, str, range));
        }

        /// <summary>
        /// 将块格式还原为默认
        /// </summary>
        private void SetDefaultformat(Button range) {
            //_content.Children.Remove(_lastRange);
        }

        /// <summary>
        /// 将设置格式的操作封装以避免触发textchanged
        /// </summary>
        /// <param name="action">设置格式的操作</param>
        private void SetContentFormat(Action action) {
            TextChanged -= RichTextBox_TextChanged;
            IsReadOnlyChanged -= RichTextBox_IsReadOnlyChanged;
            action?.Invoke();
            TextChanged += RichTextBox_TextChanged;
            IsReadOnlyChanged += RichTextBox_IsReadOnlyChanged;
        }

        private void RichTextBox_Loaded(object sender, RoutedEventArgs e) {
            SetDefaultFormat();
        }

        private void Set() {

        }

        private void SetDefaultFormat() {
            ITextCharacterFormat defaultformat = Document.GetDefaultCharacterFormat();
            defaultformat.ForegroundColor = OverallViewSettings.Instence.RichTextBoxFg;
            defaultformat.BackgroundColor = OverallViewSettings.Instence.RichTextBoxBg;
            defaultformat.Spacing = OverallViewSettings.Instence.RichTextBoxSpace;
         
            defaultformat.Size = OverallViewSettings.Instence.RichTextBoxSize;
            defaultformat.Weight = OverallViewSettings.Instence.RichTextBoxWeight;
            Document.SetDefaultCharacterFormat(defaultformat);
        }

        private string PrintDicItem(List<Rect> ranges) {
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
            ElementsLoc = new Dictionary<string, List<Rect>>();
            IsReadOnlyChanged += RichTextBox_IsReadOnlyChanged;
            Loaded += RichTextBox_Loaded;
            TextChanged += RichTextBox_TextChanged;
            Paste += RichTextBox_Paste;
            DefaultStyleKey = typeof(RichTextBox);
        }
        #endregion
    }
}