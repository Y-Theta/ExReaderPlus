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

    /// <summary>
    /// 设置工厂
    /// </summary>
    public enum ViewSettingConfigs {
        /// <summary>
        /// 富文本前景色
        /// </summary>
        RichTextBoxFg,
        /// <summary>
        /// 富文本背景色
        /// </summary>
        RichTextBoxBg,
        /// <summary>
        /// 富文本字体大小
        /// </summary>
        RichTextBoxSize,
        /// <summary>
        /// 富文本字重
        /// </summary>
        RichTextBoxWeight,
        /// <summary>
        /// 富文本段间距
        /// </summary>
        RichTextBoxLineSpace,
        /// <summary>
        /// 富文本选中颜色
        /// </summary>
        RichTextSelectBoxBg,
        /// <summary>
        /// 已掌握单词颜色
        /// </summary>
        RichTextLearned,
        /// <summary>
        /// 未掌握单词颜色
        /// </summary>
        RichTextNotLearn,
        /// <summary>
        /// 阅读器控制条位置
        /// </summary>
        ReadingPageControlBar,
        /// <summary>
        /// 富文本是否渲染
        /// </summary>
        IsRenderOn,
        /// <summary>
        /// 是否渲染已学习单词
        /// </summary>
        IsLearnedRender,
        /// <summary>
        /// 是否渲染未学习单词
        /// </summary>
        IsNotlearnRender,
    }
}
