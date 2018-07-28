using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExReaderPlus.View {
    /// <summary>
    /// 图标类型
    /// </summary>
    public enum IconKind {
        Icon,
        Rect,
        Round
    }


    public enum ViewSettingConfigs {
        /// <summary>
        /// 
        /// </summary>
        RichTextBoxFg,
        /// <summary>
        /// 
        /// </summary>
        RichTextBoxBg,
        /// <summary>
        /// 
        /// </summary>
        RichTextBoxSize,
        /// <summary>
        /// 
        /// </summary>
        RichTextBoxWeight,
        /// <summary>
        /// 
        /// </summary>
        RichTextSelectBoxBg,
        /// <summary>
        /// 
        /// </summary>
        RichTextLearned,
        /// <summary>
        /// 
        /// </summary>
        RichTextNotLearn,
        /// <summary>
        /// 
        /// </summary>
        ReadingPageControlBar,
        /// <summary>
        /// 
        /// </summary>
        IsRenderOn,
        /// <summary>
        /// 
        /// </summary>
        IsLearnedRender,
        /// <summary>
        /// 
        /// </summary>
        IsNotlearnRender,
    }
}
