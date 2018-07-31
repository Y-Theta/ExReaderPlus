using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ExReaderPlus.Models;
using ExReaderPlus.View;
using ExReaderPlus.View.Commands;
using ExReaderPlus.WordsManager;
using Windows.UI.Core;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;

namespace ExReaderPlus.ViewModels {
    public class DicPageViewModel : ViewModelBasse {

        #region Properties
        public Window window;

        /// <summary>
        /// 字典列表
        /// </summary>
        private ObservableCollection<ActionDictionary> _diclist;
        public ObservableCollection<ActionDictionary> Diclist {
            get => _diclist;
            set => SetValue(out _diclist, value, nameof(Diclist));
        }

        /// <summary>
        /// 新建字典的名称
        /// </summary>
        private string _newName;
        public string NewName {
            get => _newName;
            set => SetValue(out _newName, value, nameof(NewName));
        }

        /// <summary>
        /// 当前打开的字典
        /// </summary>
        private ActionDictionary _openedDic;
        public ActionDictionary OpenedDic {
            get => _openedDic;
            set => SetValue(out _openedDic, value, nameof(OpenedDic));
        }


        private List<AlphaKeyGroup<ActionVocabulary>> _vocabularies;
        public List<AlphaKeyGroup<ActionVocabulary>> Vocabularies {
            get => _vocabularies;
            set => SetValue(out _vocabularies, value, nameof(Vocabularies));
        }

        public CommandBase AddDicCommand { get; set; }

        public CommandBase DialogCommand { get; set; }

        public CommandBase GotoBrief { get; set; }


        public event CommandActionEventHandler CommandActions;

        public event CommandActionEventHandler DialogActions;

        public event CommandActionEventHandler DicOpAction;
        #endregion

        #region Methods
        private async void InitDiclist() {
            Task s = new Task(async () =>
            {
                while (!WordBook.Initdicready) ;
                ActionDictionary avd;
                await window.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Diclist = new ObservableCollection<ActionDictionary>();
                    avd = WordBook.GaoKao.GetActionDictionary();
                    avd.DictionaryOperation += Avd_DictionaryOperation;
                    Diclist.Add(avd);
                    avd = WordBook.CET4.GetActionDictionary();
                    avd.DictionaryOperation += Avd_DictionaryOperation;
                    Diclist.Add(avd);
                    avd = WordBook.CET6.GetActionDictionary();
                    avd.DictionaryOperation += Avd_DictionaryOperation;
                    Diclist.Add(avd);
                    avd = WordBook.KaoYan.GetActionDictionary();
                    avd.DictionaryOperation += Avd_DictionaryOperation;
                    Diclist.Add(avd);
                    avd = WordBook.TOEFL.GetActionDictionary();
                    avd.DictionaryOperation += Avd_DictionaryOperation;
                    Diclist.Add(avd);
                    avd = WordBook.IELTS.GetActionDictionary();
                    avd.DictionaryOperation += Avd_DictionaryOperation;
                    Diclist.Add(avd);
                    foreach (var customDic in WordBook.Custom) {
                        avd = customDic.GetActionDictionary();
                        avd.DictionaryOperation += Avd_DictionaryOperation;
                        Diclist.Add(avd);
                    }
                });
            });
            s.Start();
            await s;
        }

        private void Avd_DictionaryOperation(object sender, CommandArgs args) {
            switch (args.command) {
                case "Open":
                    OpenedDic = sender as ActionDictionary;
                    DicOpAction?.Invoke(sender, new CommandArgs(args.parameter, "Open"));
                    List<ActionVocabulary> avcl = new List<ActionVocabulary>();
                    foreach (var v in WordBook.GetDic(OpenedDic.DBName).Wordlist) 
                        avcl.Add(ActionVocabulary.FromVocabulary(v.Value));
                    Vocabularies = AlphaKeyGroup<ActionVocabulary>.CreateGroups(avcl, (ActionVocabulary av) => { return av.Word; }, true);
                    break;
                case "ReName":
                    DicOpAction?.Invoke(sender, new CommandArgs(args.parameter, "ReName"));
                    break;
                case "ReMove":
                    DicOpAction?.Invoke(sender, new CommandArgs(args.parameter, "ReMove"));
                    break;
                case "Select":
                    int oldindex = WordBook.SelectedDic >= 10 ? WordBook.SelectedDic - 4 : WordBook.SelectedDic;
                    WordBook.SelectedDic = (sender as ActionDictionary).DBName;
                    int newindex = WordBook.SelectedDic >= 10 ? WordBook.SelectedDic - 4 : WordBook.SelectedDic;
                    if (oldindex != newindex)
                    {
                        var oldselect = Diclist[oldindex];
                        var newselect = Diclist[newindex];
                        Diclist.Remove(oldselect);
                        Diclist.Insert(oldindex, oldselect);
                        Diclist.Remove(newselect);
                        Diclist.Insert(newindex, newselect);
                    }
                    break;
            }
        }

        private void InitCommands() {
            AddDicCommand = new CommandBase(obj => { CommandActions?.Invoke(this, new CommandArgs(obj)); });
            DialogCommand = new CommandBase(obj => { DialogActions?.Invoke(this, new CommandArgs(obj)); });
            GotoBrief = new CommandBase(obj => {
                OpenedDic = null;
                DicOpAction?.Invoke(this, new CommandArgs(obj, "Close"));
            });
        }

        public void UpdateDicinfo() {
            InitDiclist();
        }
        #endregion

        #region Constructors
        public DicPageViewModel() {
            InitDiclist();
            InitCommands();
        }
        #endregion
    }

}
