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

        private Color _richTextBoxFg;
        public Color RichTextBoxFg {
            get => _richTextBoxFg;
            set => SetValue<Color>(out _richTextBoxFg, value, nameof(RichTextBoxFg));
        }


        #endregion


        #region Methods

        #endregion

        #region Constructors
        #endregion
    }

}
