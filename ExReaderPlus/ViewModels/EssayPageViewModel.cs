using System;
using System.Timers;
using ExReaderPlus.Manage.PassageManager;
using ExReaderPlus.View.Commands;
using Windows.UI.Xaml.Media;

namespace ExReaderPlus.ViewModels {
    public class EssayPageViewModel : ViewModelBasse{
        #region Properties

        public CommandBase LoadPassage { get; set; }

        public Passage TempPassage { get; set; }


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
