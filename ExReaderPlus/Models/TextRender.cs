using ExReaderPlus.View;
using ExReaderPlus.View.Commands;
using ExReaderPlus.WordsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Media;

namespace ExReaderPlus.Models {

    /// <summary>
    /// 范围类,用于文本着色
    /// </summary>
    public class Range {
        #region Properties
        public int Start { get; set; }
        public int End { get; set; }

        #endregion

        #region Methods
        public override string ToString() {
            return string.Format("{0} - {1}", Start, End);
        }

        #endregion

        #region Constructors
        public Range(int s = 0, int e = 0) {
            Start = s;
            End = e;
        }

        public Range(Range range) {
            Start = range.Start;
            End = range.End;
        }
        #endregion
    }

    /// <summary>
    /// 文字渲染组,用于记录需要着色的文本
    /// </summary>
    public class Rendergroup {

        public HashSet<String> Words { get; set; }

        public Color TextFg { get; set; }

        public Color TextBg { get; set; }

        public ITextCharacterFormat OldFormat { get; set; }

        public Rendergroup() {

        }
    }

    public class BriefDic {
        public string Name { get; set; }

        public bool IsSys { get; set; }

        public int DicName { get; set; }

        public int WordsCount { get; set; }

        public BriefDic() { }
    }

    public class ActionVocabulary : Vocabulary {
        public HCHPointHandel PointEnter { get; set; }

        public HCHPointHandel PointExit { get; set; }


        public event CommandActionEventHandler RemCommandAction;

        public CommandBase RemCommand { get; set; }

        public void InitCommands() {
            RemCommand = new CommandBase(obj => { RemCommandAction?.Invoke(this, new CommandArgs(obj, Word)); });
        }

        public ActionVocabulary() {
            InitCommands();
        }

        public static ActionVocabulary FromVocabulary(Vocabulary v) {
            return new ActionVocabulary { Phonetic = v.Phonetic, Word = v.Word, Translation = v.Translation, Tag = v.Tag, YesorNo = v.YesorNo };
        }
    }
}
