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
                await window.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Diclist = new ObservableCollection<ActionDictionary>();
                    Diclist.Add(WordBook.GaoKao.GetActionDictionary());
                    Diclist.Add(WordBook.CET4.GetActionDictionary());
                    Diclist.Add(WordBook.CET6.GetActionDictionary());
                    Diclist.Add(WordBook.TOEFL.GetActionDictionary());
                    Diclist.Add(WordBook.KaoYan.GetActionDictionary());
                    Diclist.Add(WordBook.IELTS.GetActionDictionary());
                    foreach(var customDic in WordBook.Custom)
                        Diclist.Add(customDic.GetActionDictionary());
                });
            });
            s.Start();
            await s;
        }

        private void InitCommands() {
            AddDicCommand = new CommandBase(obj => { CommandActions?.Invoke(this, new CommandArgs(obj)); });
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
