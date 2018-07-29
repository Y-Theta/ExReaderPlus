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
        private Color _richTextBoxFg = Color.FromArgb(255, 8, 8, 8);
        public Color RichTextBoxFg {
            get => _richTextBoxFg;
            set => _richTextBoxFg = value;
        }

        /// <summary>
        /// 富文本框文字背景色
        /// </summary>
        private Color _richTextBoxBg = Colors.Transparent;
        public Color RichTextBoxBg {
            get => _richTextBoxBg;
            set => _richTextBoxBg = value;
        }

        /// <summary>
        /// 富文本框文字字体大小
        /// </summary>
        private float _richTextBoxSize = 18;
        public float RichTextBoxSize {
            get => _richTextBoxSize;
            set => _richTextBoxSize = value;
        }

        /// <summary>
        /// 富文本框段间距
        /// </summary>
        private float _richTextBoxLineSpace = 24;
        public float RichTextBoxLineSpace {
            get => _richTextBoxLineSpace;
            set => _richTextBoxLineSpace = value;
        }

        /// <summary>
        /// 富文本框文字字重
        /// </summary>
        private int _richTextBoxWeight = 400;
        public int RichTextBoxWeight {
            get => _richTextBoxWeight;
            set => _richTextBoxWeight = value;
        }

        /// <summary>
        /// 富文本框关键字背景
        /// </summary>
        private Color _richTextSelectBoxBg = Color.FromArgb(64, 5, 96, 240);
        public Color RichTextSelectBoxBg {
            get => _richTextSelectBoxBg;
            set => _richTextSelectBoxBg = value;
        }

        /// <summary>
        /// 富文本框已掌握单词背景
        /// </summary>
        private Color _richTextLearned = Color.FromArgb(64, 49, 135, 82);
        public Color RichTextLearned {
            get => _richTextLearned;
            set => _richTextLearned = value;
        }


        /// <summary>
        /// 富文本框未学习背景色
        /// </summary>
        private Color _richTextnotLearn = Color.FromArgb(64, 204, 62, 75);
        public Color RichTextNotLearn {
            get => _richTextnotLearn;
            set => _richTextnotLearn = value;
        }

        /// <summary>
        /// 阅读界面控制条位置
        /// </summary>
        private Thickness _readingPageControlBar = new Thickness(8, 0, 8, 32);
        public Thickness ReadingPageControlBar {
            get => _readingPageControlBar;
            set => _readingPageControlBar = value;
        }

        /// <summary>
        /// 是否为单词着色
        /// </summary>
        private bool _isRenderOn = true;
        public bool IsRenderOn {
            get => _isRenderOn;
            set => _isRenderOn = value;
        }

        /// <summary>
        /// 是否为已掌握单词着色
        /// </summary>
        private bool _isLearnedRender = true;
        public bool IsLearnedRender {
            get => _isLearnedRender;
            set => _isLearnedRender = value;
        }

        /// <summary>
        /// 是否为未掌握单词着色
        /// </summary>
        private bool _isNotlearnRender = true;
        public bool IsNotlearnRender {
            get => _isNotlearnRender;
            set => _isNotlearnRender = value;
        }
        #endregion


        #region Methods
        #endregion

        #region Constructors

        #endregion
    }

}
