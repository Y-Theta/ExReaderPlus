using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExReaderPlus.View.Commands {
    /// <summary>
    /// 命令基类，用于控件中使用命令
    /// </summary>
    public class CommandBase : ICommand {
        #region Properties
        public event EventHandler CanExecuteChanged;
        public delegate void CommandAction(object parameter);

        private CommandAction action;
        public event CommandAction Commandaction {
            add => action = value;
            remove => action -= value;
        }
        #endregion

        #region Methods
        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            action?.Invoke(parameter);
        }
        #endregion

        #region Constructors
        public CommandBase(CommandAction act) {
            Commandaction += act;
        }

        public CommandBase() {
        }
        #endregion

    }

}
