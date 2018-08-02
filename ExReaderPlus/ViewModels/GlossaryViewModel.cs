using ExReaderPlus.Manage.PassageManager;
using ExReaderPlus.Models;
using ExReaderPlus.View;
using ExReaderPlus.View.Commands;
using ExReaderPlus.View.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExReaderPlus.ViewModels {
    public class GlossaryViewModel : ViewModelBasse {
        #region Properties

        private ObservableCollection<ActionPassage> _diclist;
        public ObservableCollection<ActionPassage> Diclist {
            get => _diclist;
            set => SetValue(out _diclist, value, nameof(Diclist));
        }

        public CommandBase Option { get; set; }

        public event CommandActionEventHandler OptionAction;
        #endregion

        #region Methods
        public void InitRes() {
            Diclist.Clear();
            ActionPassage ac = null;
            foreach (var ps in PassageManage.GetAllPassagesInfo())
            {
                ac = ActionPassage.FromDBPassage(ps);
                ac.PassageOperation += Ac_PassageOperation;
                Diclist.Add(ac);
            }
        }

        private async void Ac_PassageOperation(object sender, CommandArgs args) {
            switch (args.command) {
                case "Open":
                    var evm = App.Current.Resources["EssayPageViewModel"] as EssayPageViewModel;
                    var mvm = App.Current.Resources["MainPageViewModel"] as MainPageViewModel;
                    mvm.Navigate.Execute(App.Current.Resources["EssayPage"] as EssayPage);
                    evm.LoadPassage(await PassageManage.GetPassage((sender as UserDictionary.Passage)));
                    break;
                case "Remove":
                    Diclist.Remove((sender as ActionPassage));
                    PassageManage.DeletePassageInfoAndPassage((sender as UserDictionary.Passage));
                    break;
            }
        }

        private async void GlossaryViewModel_OptionAction(object sender, CommandArgs args) {
            switch (args.parameter)
            {
                case "Add":
                    Passage pass = null;
                    try
                    {
                        pass = await FileManage.FileManage.Instence.OpenFile();
                    }
                    catch (Exception e)
                    {

                    }
                    if (pass is null)
                        return;
                    else
                    {
                        PassageManage.SavaPassageInfoAndPassage(pass);
                    }
                    InitRes();
                    break;
            }
        }

        private void InitCommand() {
            Option = new CommandBase(obj => { OptionAction?.Invoke(this, new CommandArgs(obj,nameof(Option))); });
        }
        #endregion

        #region Constructors
        public GlossaryViewModel() {
            _diclist = new ObservableCollection<ActionPassage>();
            InitCommand();
            InitRes();
            OptionAction += GlossaryViewModel_OptionAction;
        }

        #endregion
    }

}
