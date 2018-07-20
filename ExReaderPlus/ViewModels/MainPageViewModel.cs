using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExReaderPlus.ViewModels {
    public class MainPageViewModel : ViewModelBasse {

        #region Properties
        private string _userIcon = @"ms-appx:///Assets/Cybran_Y_T.jpg";
        public string UserIcon {
            get => _userIcon;
            set => SetValue<string>(out _userIcon, value, "UserIcon");
        }
        #endregion

        #region Methods
        #endregion

        #region Constructors
        #endregion

    }
}
