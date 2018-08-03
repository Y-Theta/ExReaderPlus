using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ExReaderPlus.View.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AboutPage : Page {
        public string CIntroduction { get; set; }

        public AboutPage() {
            CIntroduction = "这是一款生产实习完成的项目\n" +
                "项目的idea并不是本次开发人员原创的，但是作为二次开发者，" +
                "我们将原软件的大部分基本功能都保留了下来，去掉了一些不" +
                "合理的功能,扩展了其他的一些相关功能，优化了UI，提升了用户阅读体验。\n" +
                "目前我们支持的功能有：\n" +
                "1 从TXT导入文章，TXT编码要为Unicode可识别 \n" +
                "2 对文章内容（英文部分）中在词库中的词进行着色 \n" +
                "3 对文章中的单词提供离线的实时翻译，但支持不全 \n" +
                "4 用户可以新建词库，并将遇到的单词插入新词库 \n";
            InitializeComponent();
            Loaded += AboutPage_Loaded;
        }

        private void AboutPage_Loaded(object sender, RoutedEventArgs e) {
            EnterAnimation.Begin();
        }
    }
}
