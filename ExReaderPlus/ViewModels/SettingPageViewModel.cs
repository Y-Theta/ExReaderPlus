using ExReaderPlus.View;
using ExReaderPlus.View.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExReaderPlus.ViewModels {
    public class SettingPageViewModel : ViewModelBasse {
        #region Properties
        public CommandBase SettingCommand { get; set; }


        public event CommandActionEventHandler SettingCommandAction;

        #endregion

        #region Methods
        private void InitCommand() {
            SettingCommand = new CommandBase(obj => { SettingCommandAction?.Invoke(this, new CommandArgs(obj)); });
        }

        private void SettingPageViewModel_SettingCommandAction(object sender, CommandArgs args) {
            switch (args.parameter) {

                case "ThemeSwitch":
                    break;
            }
        }
        #endregion

        #region Constructors
        public SettingPageViewModel() {
            InitCommand();
            SettingCommandAction += SettingPageViewModel_SettingCommandAction;
        }
        #endregion
    }

}
