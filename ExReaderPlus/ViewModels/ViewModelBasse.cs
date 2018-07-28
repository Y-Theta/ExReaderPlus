using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExReaderPlus.ViewModels {
    public class ViewModelBasse : INotifyPropertyChanged {
        #region Properties
        #endregion

        #region Methods
        #endregion

        #region Constructors
        #endregion

        #region InterfaceCompletion
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetValue<T>(out T newValue, T value, string propname, Action action = null) {
            newValue = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
            action?.Invoke();
        }
        #endregion
    }

}
