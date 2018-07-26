using System;
using System.Collections.Generic;
using System.Timers;
using ExReaderPlus.Manage.PassageManager;
using ExReaderPlus.View;
using ExReaderPlus.View.Commands;
using Windows.UI.Xaml.Media;

namespace ExReaderPlus.ViewModels {
    public class EssayPageViewModel : ViewModelBasse {
        #region Properties
        public Passage TempPassage { get; set; }
        #endregion

        #region Commands&Events

        public CommandBase LoadPassage { get; set; }

        public CommandBase TurnPage { get; set; }

        public CommandBase SizeText { get; set; }

        public CommandBase ChangeMode { get; set; }


        public event CommandActionEventHandler ControlCommand;

        public event EventHandler PassageLoaded;
        #endregion

        #region Methods
        private void InitCommand() {
            LoadPassage = new CommandBase(async obj =>
            {
                TempPassage = null;
                TempPassage = await FileManage.FileManage.Instence.DeSerializeFile();
                while (TempPassage is null) ;
                PassageLoaded?.Invoke(this, EventArgs.Empty);
            });
            TurnPage = new CommandBase(obj => { ControlCommand?.Invoke(this, new CommandArgs(obj, nameof(TurnPage))); });
            SizeText = new CommandBase(obj => { ControlCommand?.Invoke(this, new CommandArgs(obj, nameof(SizeText))); });
            ChangeMode = new CommandBase(obj => { ControlCommand?.Invoke(this, new CommandArgs(obj, nameof(ChangeMode))); });
        }

        private void LoadRes() {

        }
        #endregion

        #region Constructors
        public EssayPageViewModel() {
            InitCommand();
            LoadRes();
        }
        #endregion
    }

}
