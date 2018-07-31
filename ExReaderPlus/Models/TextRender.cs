using ExReaderPlus.View;
using ExReaderPlus.View.Commands;
using ExReaderPlus.WordsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.Collation;
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

    public class ActionDictionary {

        #region Properties
        public string Name { get; set; }

        public int WordsCount { get; set; }

        public int DBName { get; set; }

        public bool IsSys { get; set; }

        public int LearnedWords { get; set; }

        public CommandBase Open { get; set; }

        public CommandBase ReName { get; set; }

        public CommandBase ReMove { get; set; }

        public CommandBase Select { get; set; }

        #endregion

        #region Events

        public event CommandActionEventHandler DictionaryOperation;
        #endregion

        #region Methods
        private void InitCommand() {
            Open = new CommandBase(obj => {
                DictionaryOperation?.Invoke(this, new CommandArgs(obj, nameof(Open)));
            });
            ReName = new CommandBase(obj => { DictionaryOperation?.Invoke(this, new CommandArgs(obj, nameof(ReName))); });
            ReMove = new CommandBase(obj => { DictionaryOperation?.Invoke(this, new CommandArgs(obj, nameof(ReMove))); });
            Select = new CommandBase(obj => { DictionaryOperation?.Invoke(this, new CommandArgs(obj, nameof(Select))); });

        }
        #endregion

        public ActionDictionary() {
            InitCommand();
        }


    }


    public class AlphaKeyGroup<T> {
        const string GlobeGroupKey = "\uD83C\uDF10";

        public string Key { get; private set; }

        public List<T> InternalList { get; private set; }

        public AlphaKeyGroup(string key) {
            Key = key;
            InternalList = new List<T>();
        }

        private static List<AlphaKeyGroup<T>> CreateDefaultGroups(CharacterGroupings slg) {
            List<AlphaKeyGroup<T>> list = new List<AlphaKeyGroup<T>>();

            foreach (CharacterGrouping cg in slg)
            {
                if (cg.Label == "")
                    continue;
                if (cg.Label == "...")
                {
                    list.Add(new AlphaKeyGroup<T>(GlobeGroupKey));
                }
                else
                {
                    list.Add(new AlphaKeyGroup<T>(cg.Label));
                }
            }

            return list;
        }

        public static List<AlphaKeyGroup<T>> CreateGroups(IEnumerable<T> items, Func<T, string> keySelector, bool sort) {
            CharacterGroupings slg = new CharacterGroupings("EN");
            List<AlphaKeyGroup<T>> list = CreateDefaultGroups(slg);

            foreach (T item in items)
            {
                int index = 0;
                {
                    string label = slg.Lookup(keySelector(item));
                    index = list.FindIndex(alphakeygroup => (alphakeygroup.Key.Equals(label, StringComparison.CurrentCulture)));
                }

                if (index >= 0 && index < list.Count)
                {
                    list[index].InternalList.Add(item);
                }
            }

            if (sort)
            {
                foreach (AlphaKeyGroup<T> group in list)
                {
                    group.InternalList.Sort((c0, c1) => { return keySelector(c0).CompareTo(keySelector(c0)); });
                }
            }

            return list;
        }
    }
}
