using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using ExReaderPlus.Manage.PassageManager;
using ExReaderPlus.Models;
using ExReaderPlus.View;
using ExReaderPlus.View.Commands;
using Windows.UI.Xaml.Media;

namespace ExReaderPlus.ViewModels {
    public class EssayPageViewModel : ViewModelBasse {
        #region Properties
        public Passage TempPassage { get; set; }


        /// <summary>
        /// 关键字，需要渲染的关键字组
        /// </summary>
        private HashSet<string> _keyWords;
        public HashSet<string> KeyWords {
            get => _keyWords;
            set => _keyWords = value;
        }

        

        /// <summary>
        /// 侧边栏列表
        /// </summary>
        private ObservableCollection<ActionVocabulary> _keywordlist;
        public ObservableCollection<ActionVocabulary> Keywordlist {
            get => _keywordlist;
            set => _keywordlist = value;
        }

        private bool _wordsLearned = true;
        public bool WordsLearned {
            get => _wordsLearned;
            set {
                _wordsLearned = value;
                if (value)
                    AddWordLearned();
                else
                    RemoveWordLearned();
            }
        }

        private bool _wordsUnLearned = true;
        public bool WordsUnLearned {
            get => _wordsUnLearned;
            set {
                _wordsUnLearned = value;
                if (value)
                    AddWordUnLearned();
                else
                    RemoveWordUnLearned();
            }
        }
        #endregion

        #region Commands&Events

        public CommandBase LoadPassage { get; set; }

        public CommandBase ControlBarCommand { get; set; }


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
            ControlBarCommand = new CommandBase(obj => { ControlCommand?.Invoke(this, new CommandArgs(obj, nameof(ControlBarCommand))); });
        }

        private void InitRes() {

        }

        private void LoadRes() {
            KeyWords = new HashSet<string>();
            Keywordlist = new ObservableCollection<ActionVocabulary>();
        }

        private void AddWordLearned() {

        }

        private void RemoveWordLearned() {

        }

        private void AddWordUnLearned() {

        }

        private void RemoveWordUnLearned() {

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
