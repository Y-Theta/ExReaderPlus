using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ExReaderPlus.Manage.PassageManager;
using ExReaderPlus.View.Commands;

namespace ExReaderPlus.ViewModels {
    public class EssayPageViewModel : ViewModelBasse{
        #region Properties
        private Timer _passagetimer;

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
                _passagetimer.Enabled = true;
            });
        }

        private void InitTimer() {
            _passagetimer = new Timer { Interval = 1000 };
            _passagetimer.Elapsed += _passagetimer_Elapsed;
        }

        private void _passagetimer_Elapsed(object sender, ElapsedEventArgs e) {
            while (TempPassage is null) ;
            PassageLoaded?.Invoke(this, EventArgs.Empty);
            _passagetimer.Enabled = false;
        }
        #endregion

        #region Constructors
        public EssayPageViewModel() {
            InitTimer();
            InitCommand();
        }
        #endregion
    }

}
