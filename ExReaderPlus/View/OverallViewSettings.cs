using System;
using ExReaderPlus.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace ExReaderPlus.View {
    [Serializable]
    public class OverallViewSettings : ViewModelBasse{
        private static OverallViewSettings _instence;
        /// <summary>
        /// 用户界面设置存储
        /// </summary>
        public static OverallViewSettings Instence {
            get {
                if (_instence is null)
                    _instence = new OverallViewSettings();
                return _instence;
            }
        }

        #region Properties

        private Color _richTextBoxFg = Color.FromArgb(255, 8, 8, 8);
        public Color RichTextBoxFg {
            get => _richTextBoxFg;
            set => SetValue<Color>(out _richTextBoxFg, value, nameof(RichTextBoxFg));
        }

        private Color _richTextBoxBg = Colors.Transparent;
        public Color RichTextBoxBg {
            get => _richTextBoxBg;
            set => SetValue<Color>(out _richTextBoxBg, value, nameof(RichTextBoxBg));
        }

        private float _richTextBoxSize = 12;
        public float RichTextBoxSize {
            get => _richTextBoxSize;
            set => SetValue<float>(out _richTextBoxSize, value, nameof(RichTextBoxSize));
        }

        private int _richTextBoxWeight = 400;
        public int RichTextBoxWeight {
            get => _richTextBoxWeight;
            set => SetValue<int>(out _richTextBoxWeight, value, nameof(RichTextBoxWeight));
        }

        private Color _richTextSelectBoxFg = Color.FromArgb(255, 8, 8, 8);
        public Color RichTextSelectBoxFg {
            get => _richTextSelectBoxFg;
            set => SetValue<Color>(out _richTextSelectBoxFg, value, nameof(RichTextSelectBoxFg));
        }

        private Color _richTextSelectBoxBg = Color.FromArgb(255, 115, 135, 86);
        public Color RichTextSelectBoxBg {
            get => _richTextSelectBoxBg;
            set => SetValue<Color>(out _richTextSelectBoxBg, value, nameof(RichTextSelectBoxBg));
        }
        #endregion


        #region Methods

        #endregion

        #region Constructors
        #endregion
    }

}
