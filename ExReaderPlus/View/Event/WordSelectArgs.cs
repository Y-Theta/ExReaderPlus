using ExReaderPlus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace ExReaderPlus.View {
    public delegate void WordSelectEventHandler(object sender, WordSelectArgs args);

    public class WordSelectArgs : EventArgs {
        #region Properties
        public string SelectedWord { get; set; }

        public Range TextRange { get; set; }

        public Rect TextBounds { get; set; }
        #endregion

        #region Methods
        #endregion

        #region Constructors
        public WordSelectArgs(Rect rect ,string str = null, Range range = null) {
            SelectedWord = str;
            TextRange = range;
            TextBounds = rect;
        }
        #endregion
    }

}
