using ExReaderPlus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace ExReaderPlus.View {
    public delegate void CommandActionEventHandler(object sender, CommandArgs args);

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
