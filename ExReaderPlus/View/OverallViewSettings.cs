using System;
using ExReaderPlus.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using System.Diagnostics;
using Windows.UI.ViewManagement;

namespace ExReaderPlus.View {
    [Serializable]
    public class OverallViewSettings {

        #region Properties
        /// <summary>
        /// 富文本框文字前景色
        /// </summary>
        [field:NonSerialized]
        public Color RichTextBoxFg = Color.FromArgb(255, 8, 8, 8);
        private string _richTextBoxFg;

        /// <summary>
        /// 富文本框文字背景色
        /// </summary>
        [field: NonSerialized]
        public Color RichTextBoxBg = Colors.Transparent;
        private string _richTextBoxBg;


        /// <summary>
        /// 富文本框文字字体大小
        /// </summary>
        public float RichTextBoxSize = 18;

        /// <summary>
        /// 富文本框段间距
        /// </summary>
        public float RichTextBoxLineSpace = 24;

        /// <summary>
        /// 富文本框文字字重
        /// </summary>
        public int RichTextBoxWeight = 400;

        /// <summary>
        /// 富文本框关键字背景
        /// </summary>
        [field: NonSerialized]
        public Color RichTextSelectBoxBg = Color.FromArgb(160, 5, 96, 240);
        private string _richTextSelectBoxBg;


        /// <summary>
        /// 富文本框已掌握单词背景
        /// </summary>
        [field: NonSerialized]
        public Color RichTextLearned = Color.FromArgb(160, 49, 135, 82);
        private string _richTextLearned;


        /// <summary>
        /// 富文本框未学习背景色
        [field: NonSerialized]
        public Color RichTextNotLearn = Color.FromArgb(160, 204, 62, 75);
        private string _richTextNotLearn;


        /// <summary>
        /// 阅读界面控制条位置
        /// </summary>
        [field:NonSerialized]
        public Thickness ReadingPageControlBar = new Thickness(8, 0, 8, 32);
        private string _readingPageControlBar;


        /// <summary>
        /// 是否为单词着色
        /// </summary>
        public bool IsRenderOn = true;

        /// <summary>
        /// 是否为已掌握单词着色
        /// </summary>
        public bool IsLearnedRender = true;

        /// <summary>
        /// 是否为未掌握单词着色
        /// </summary>
        public bool IsNotlearnRender = true;

        /// <summary>
        /// 当前选中的字典
        /// </summary>
        public int SelectedDic = 0;

        /// <summary>
        /// 当前应用主题模式
        /// </summary>
        public bool AppThemeMode = true;
        #endregion


        #region Methods
        public void StreamCode() {
            _richTextBoxFg = FormatColor(RichTextBoxFg);
            _richTextBoxBg = FormatColor(RichTextBoxBg);
            _richTextSelectBoxBg = FormatColor(RichTextSelectBoxBg);
            _richTextLearned = FormatColor(RichTextLearned);
            _richTextNotLearn = FormatColor(RichTextNotLearn);
            _readingPageControlBar = FormatThickness(ReadingPageControlBar);
        }

        public void StreamDeCode() {
            RichTextBoxFg = ReFormatColor(_richTextBoxFg);
            RichTextBoxBg = ReFormatColor(_richTextBoxBg);
            RichTextSelectBoxBg = ReFormatColor(_richTextSelectBoxBg);
            RichTextLearned = ReFormatColor(_richTextLearned);
            RichTextNotLearn = ReFormatColor(_richTextNotLearn);
            ReadingPageControlBar = ReFormatThickness(_readingPageControlBar);
        }

        private string FormatThickness(Thickness thick) {
            return String.Format("{0},{1},{2},{3}", thick.Left, thick.Top, thick.Right, thick.Bottom);
        }

        private string FormatColor(Color color) {
            return String.Format("{0},{1},{2},{3}", color.A, color.R, color.G, color.B);
        }

        private Color ReFormatColor (string str) {
            string[] vectors = str.Split(',');
            return Color.FromArgb(
                Convert.ToByte(vectors[0]),
                Convert.ToByte(vectors[1]),
                Convert.ToByte(vectors[2]),
                Convert.ToByte(vectors[3]));
        }

        private Thickness ReFormatThickness(string str) {
            string[] vectors = str.Split(',');
            return new Thickness(
                Convert.ToDouble(vectors[0]),
                Convert.ToDouble(vectors[1]),
                Convert.ToDouble(vectors[2]),
                Convert.ToDouble(vectors[3]));
        }
        #endregion

        #region Constructors
        public OverallViewSettings() { }
        #endregion
    }

}
