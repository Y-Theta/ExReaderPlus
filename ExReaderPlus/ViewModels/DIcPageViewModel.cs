using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ExReaderPlus.Models;
using ExReaderPlus.View;
using ExReaderPlus.View.Commands;
using ExReaderPlus.WordsManager;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace ExReaderPlus.ViewModels {
    public class DicPageViewModel : ViewModelBasse {

        #region Properties
        public Window window;

        private ObservableCollection<ActionDictionary> _diclist;
        public ObservableCollection<ActionDictionary> Diclist {
            get => _diclist;
            set => SetValue(out _diclist, value, nameof(Diclist));
        }

        public CommandBase AddDicCommand { get; set; }

        public event CommandActionEventHandler CommandActions;
        #endregion

        #region Methods
        private async void InitDiclist() {
            Task s = new Task(async () =>
            {
                while (!WordBook.Initdicready) ;
                ActionDictionary ad;
                await window.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Diclist = new ObservableCollection<ActionDictionary>();
                    ad = WordBook.GaoKao.GetActionDictionary();
                    ad.DictionaryOperation += Ad_DictionaryOperation;
                    Diclist.Add(ad);
                    ad = WordBook.CET4.GetActionDictionary();
                    ad.DictionaryOperation += Ad_DictionaryOperation;
                    Diclist.Add(ad);
                    ad = WordBook.CET6.GetActionDictionary();
                    ad.DictionaryOperation += Ad_DictionaryOperation;
                    Diclist.Add(ad);
                    ad = WordBook.TOEFL.GetActionDictionary();
                    ad.DictionaryOperation += Ad_DictionaryOperation;
                    Diclist.Add(ad);
                    ad = WordBook.KaoYan.GetActionDictionary();
                    ad.DictionaryOperation += Ad_DictionaryOperation;
                    Diclist.Add(ad);
                    ad = WordBook.IELTS.GetActionDictionary();
                    ad.DictionaryOperation += Ad_DictionaryOperation;
                    Diclist.Add(ad);
                    foreach (var customDic in WordBook.Custom) {
                        ad = customDic.GetActionDictionary();
                        ad.DictionaryOperation += Ad_DictionaryOperation;
                        Diclist.Add(ad);
                    }
                });
            });
            s.Start();
            await s;
        }

        private void Ad_DictionaryOperation(object sender, CommandArgs args) {
            switch (args.command)
            {
                case "Open":
                    break;
                case "ReName":
                    break;
                case "ReMove":
                    break;
                default: break;
            }
        }

        private void InitCommands() {
            AddDicCommand = new CommandBase(obj => {
                // TODO:
                CommandActions?.Invoke(this, new CommandArgs(obj));
            });
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
