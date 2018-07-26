using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExReaderPlus.Models;
using ExReaderPlus.WordsManager;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ExReaderPlus.ViewModels {
    public class DicPageViewModel : ViewModelBasse {

        #region Properties
        public Window window;

        private ObservableCollection<BriefDic> _diclist;
        public ObservableCollection<BriefDic> Diclist {
            get => _diclist;
            set => SetValue<ObservableCollection<BriefDic>>(out _diclist, value, nameof(Diclist));
        }
        #endregion

        #region Methods
        private async void InitDiclist() {
            Task s = new Task(async () =>
            {
                while (!WordBook.Initdicready) ;
                await window.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Diclist = new ObservableCollection<BriefDic>();
                    Diclist.Add(WordBook.GaoKao.GetBriefDic());
                    Diclist.Add(WordBook.CET4.GetBriefDic());
                    Diclist.Add(WordBook.CET6.GetBriefDic());
                    Diclist.Add(WordBook.TOEFL.GetBriefDic());
                    Diclist.Add(WordBook.KaoYan.GetBriefDic());
                    Diclist.Add(WordBook.IELTS.GetBriefDic());
                });
            });
            s.Start();
            await s;
        }
        #endregion

        #region Constructors
        public DicPageViewModel() {
            InitDiclist();
        }
        #endregion
    }

}
