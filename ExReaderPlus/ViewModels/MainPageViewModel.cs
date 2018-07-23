using ExReaderPlus.View.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace ExReaderPlus.ViewModels {
    public class MainPageViewModel : ViewModelBasse {

        #region Properties
        private string _userIcon = @"ms-appx:///Assets/Cybran_Y_T.jpg";
        public string UserIcon {
            get => _userIcon;
            set => SetValue<string>(out _userIcon, value, nameof(UserIcon));
        }

        private Brush _frameBg;
        public Brush FrameBg {
            get => _frameBg;
            set => SetValue<Brush>(out _frameBg, value, nameof(FrameBg));
        }

        public CommandBase Navigate { get; set; }
        /// <summary>
        /// 导航命令消息回调
        /// </summary>
        public event EventHandler OnNavigate;
        #endregion

        #region Methods
        private void InitCommand() {
            Navigate = new CommandBase();
            Navigate.Commandaction += Navigate_Commandaction;
        }

        private void Navigate_Commandaction(object parameter) {
            OnNavigate?.Invoke(parameter, EventArgs.Empty);
        }
        #endregion

        #region Constructors
        public MainPageViewModel() {
            InitCommand();
        }
        #endregion

    }
}
