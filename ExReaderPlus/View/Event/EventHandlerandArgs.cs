using ExReaderPlus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace ExReaderPlus.View {
    /// <summary>
    /// 命令回调
    /// </summary>
    public delegate void CommandActionEventHandler(object sender, CommandArgs args);

    /// <summary>
    /// 着色变更回调
    /// </summary>
    public delegate void RenderChangeEventHandler(object sender, string name, bool value);

    /// <summary>
    /// 点击内容控件鼠标进入回调
    /// </summary>
    public delegate void HCHPointHandel(object sender);

    public class CommandArgs : EventArgs {
        #region Properties
        public object parameter { get; set; }

        public string command { get; set; }
        #endregion

        #region Methods
        #endregion

        #region Constructors
        public CommandArgs(object para, string str = null) {
            parameter = para;
            command = str;
        }
        #endregion
    }

}
