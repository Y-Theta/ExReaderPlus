using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Timers;
using ExReaderPlus.Manage;
using ExReaderPlus.Manage.PassageManager;
using ExReaderPlus.Models;
using ExReaderPlus.View;
using ExReaderPlus.View.Commands;
using ExReaderPlus.WordsManager;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ExReaderPlus.ViewModels {
    public class EssayPageViewModel : ViewModelBasse {
        #region Properties
        public Passage TempPassage { get; set; }

        public OverSettingService _settingService;

        private float _richTextSize;
        /// <summary>
        /// 字体大小
        /// </summary>
        public float RichTextSize {
            get => _richTextSize;
            set {
                SetValue(out _richTextSize, value, nameof(RichTextSize));
                _settingService.SetValue(ViewSettingConfigs.RichTextBoxSize, value);
            }
        }

        private float _richTextBoxLineSpace;
        /// <summary>
        /// 行间距
        /// </summary>
        public float RichTextBoxLineSpace {
            get => _richTextBoxLineSpace;
            set {
                if (value > 45)
                    value = 45;
                if (value < 24)
                    value = 24;
                SetValue(out _richTextBoxLineSpace, value, nameof(RichTextBoxLineSpace));
                _settingService.SetValue(ViewSettingConfigs.RichTextBoxLineSpace, value);
            }
        }

        private Thickness _controlBarThickness;
        /// <summary>
        /// 控制条位置
        /// </summary>
        public Thickness ControlBarThickness {
            get => _controlBarThickness;
            set {
                SetValue(out _controlBarThickness, value, nameof(ControlBarThickness));
                _settingService.SetValue(ViewSettingConfigs.ReadingPageControlBar, value);
            }
        }

        /// <summary>
        /// 关键字背景
        /// </summary>
        private SolidColorBrush _normalBg;
        public SolidColorBrush NormalBg {
            get => _normalBg;
            set {
                SetValue(out _normalBg, value, nameof(NormalBg));
                _settingService.SetValue(ViewSettingConfigs.RichTextSelectBoxBg, value.Color);
            }
        }

        /// <summary>
        /// 已掌握单词颜色
        /// </summary>
        private SolidColorBrush _learned;
        public SolidColorBrush LearnedBg {
            get => _learned;
            set {
                SetValue(out _learned, value, nameof(LearnedBg));
                _settingService.SetValue(ViewSettingConfigs.RichTextLearned, value.Color);
            }
        }

        /// <summary>
        /// 未掌握单词颜色
        /// </summary>
        private SolidColorBrush _notLearn;
        public SolidColorBrush NotLearnBg {
            get => _notLearn;
            set {
                SetValue(out _notLearn, value, nameof(NotLearnBg));
                _settingService.SetValue(ViewSettingConfigs.RichTextNotLearn, value.Color);
            }
        }

        /// <summary>
        /// 当前页面情况
        /// </summary>
        private string _pageinfo;
        public string PageInfo {
            get => _pageinfo;
            set => SetValue(out _pageinfo, value, nameof(PageInfo));
        }

        /// <summary>
        /// 当前词数
        /// </summary>
        private string _wordcount;
        public string WordCount {
            get => _wordcount;
            set => SetValue(out _wordcount, value, nameof(WordCount));
        }

        /// <summary>
        /// 已掌握关键字
        /// </summary>
        private HashSet<string> _keywordlearn;
        public HashSet<string> KeyWordLearn {
            get => _keywordlearn;
            set => _keywordlearn = value;
        }

        /// <summary>
        /// 未掌握关键字
        /// </summary>
        private HashSet<string> _keywordnotlearn;
        public HashSet<string> KeyWordNotLearn {
            get => _keywordnotlearn;
            set =>_keywordnotlearn = value; 
        }

        /// <summary>
        /// 侧边栏列表
        /// </summary>
        private ObservableCollection<ActionVocabulary> _keywordlist;
        public ObservableCollection<ActionVocabulary> KeywordList {
            get => _keywordlist;
            set => SetValue<ObservableCollection<ActionVocabulary>>(out _keywordlist, value, nameof(KeywordList));
        }

        /// <summary>
        /// 单词列表显示模式
        /// </summary>
        private int _shownState;
        public int ShownState {
            get => _shownState;
            set {
                SetValue(out _shownState, value, nameof(ShownState));
                ShownStateChanged?.Invoke(null);
            }
        }

        /// <summary>
        /// 是否渲染已掌握单词
        /// </summary>
        private bool _learnedColor;
        public bool LearnedColor {
            get => _learnedColor;
            set {
                SetValue(out _learnedColor, value, nameof(LearnedColor), LearnedColorChnage);
                _settingService.SetValue(ViewSettingConfigs.IsLearnedRender, value);
            }
        }
        private void LearnedColorChnage() {
            OnRenderChange.Invoke(this, "Lea", _learnedColor);
        }

        /// <summary>
        /// 是否渲未掌握单词
        /// </summary>
        private bool _notlearnColor;
        public bool NotlearnColor {
            get => _notlearnColor;
            set {
                SetValue(out _notlearnColor, value, nameof(NotlearnColor), NotlearnColorChange);
                _settingService.SetValue(ViewSettingConfigs.IsNotlearnRender, value);
            }
        }
        private void NotlearnColorChange() {
            OnRenderChange.Invoke(this, "Not", _notlearnColor);
        }

        #endregion

        #region Commands&Events

        public CommandBase LoadPassage { get; set; }

        public CommandBase ControlBarCommand { get; set; }

        public CommandBase ShareCommand { get; set; }


        /// <summary>
        /// 着色需求变换
        /// </summary>
        public event RenderChangeEventHandler OnRenderChange;

        public event CommandActionEventHandler ControlCommand;

        public event HCHPointHandel ShownStateChanged;

        public event HCHPointHandel WordStateChanged;

        public event EventHandler PassageLoaded;
        #endregion

        #region Methods

        #region InterfaceMethods

        public void ClearKeyWordHashSet() {
            KeyWordLearn.Clear();
            KeyWordNotLearn.Clear();
        }

        public void SetStateBarButtonFg(Color color) {
            var TitleBar = ApplicationView.GetForCurrentView().TitleBar;
            TitleBar.ButtonForegroundColor = color;
        }

        public async void UpdateKeywordList(ActionVocabulary voc) {
            ActionVocabulary avb = voc;
            avb.YesorNo = voc.YesorNo == 0 ? 1 : 0;
            await WordBook.ChangeWordStatePenetrateAsync(avb.Word, avb.YesorNo);
            WordStateChanged?.Invoke(avb);
            int index = KeywordList.IndexOf(voc);
            KeywordList.Remove(voc);
            if (ShownState.Equals(0))
                KeywordList.Insert(index, avb);
        }

        public void LoadKeywordList(CommandActionEventHandler action = null, HCHPointHandel pointact = null, HCHPointHandel pointexit = null) {
            KeywordList.Clear();
            if (ShownState.Equals(0))
            {
                foreach (var v in KeyWordLearn)
                    InitKeywordAction(v, action, pointact, pointexit);
                foreach (var v in KeyWordNotLearn)
                    InitKeywordAction(v, action, pointact, pointexit);

            }
            else if (ShownState.Equals(1))
            {
                foreach (var v in KeyWordLearn)
                    InitKeywordAction(v, action, pointact, pointexit);
            }
            else
            {
                foreach (var v in KeyWordNotLearn)
                    InitKeywordAction(v, action, pointact, pointexit);
            }
        }

        private void InitKeywordAction(string str, CommandActionEventHandler action, HCHPointHandel pointact, HCHPointHandel pointexit) {
            var acb = ActionVocabulary.FromVocabulary(WordBook.GetDicNow().Wordlist[str]);
            acb.RemCommandAction += action;
            acb.PointEnter += pointact;
            acb.PointExit += pointexit;
            KeywordList.Add(acb);
        }

        #endregion

        private void InitCommand() {
            LoadPassage = new CommandBase(async obj =>
            {
                TempPassage = null;
                TempPassage = await FileManage.FileManage.Instence.OpenFile();
                if (TempPassage is null)
                    return;
                else
                    PassageLoaded?.Invoke(this, EventArgs.Empty);
            });
            ShareCommand = new CommandBase( obj =>
            {
                if (obj.Equals("0"))
                    Debug.WriteLine("sss");
                else
                    Debug.WriteLine("aaa");
            });
            ControlBarCommand = new CommandBase(obj => { ControlCommand?.Invoke(this, new CommandArgs(obj, nameof(ControlBarCommand))); });
        }

        private void InitRes() {

        }

        private void LoadRes() {
            KeyWordLearn = new HashSet<string>();
            KeyWordNotLearn = new HashSet<string>();
            KeywordList = new ObservableCollection<ActionVocabulary>();

            _richTextSize = (float)_settingService.GetValue(ViewSettingConfigs.RichTextBoxSize);
            _controlBarThickness = (Thickness)_settingService.GetValue(ViewSettingConfigs.ReadingPageControlBar);
            _normalBg = new SolidColorBrush((Color)_settingService.GetValue(ViewSettingConfigs.RichTextSelectBoxBg));
            _learned = new SolidColorBrush((Color)_settingService.GetValue(ViewSettingConfigs.RichTextLearned));
            _notLearn = new SolidColorBrush((Color)_settingService.GetValue(ViewSettingConfigs.RichTextNotLearn));
            _richTextBoxLineSpace = (float)_settingService.GetValue(ViewSettingConfigs.RichTextBoxLineSpace);
            _notlearnColor = (bool)_settingService.GetValue(ViewSettingConfigs.IsNotlearnRender);
            _learnedColor = (bool)_settingService.GetValue(ViewSettingConfigs.IsLearnedRender);

            ShownState = 0;
        }

        #endregion

        #region Constructors
        public EssayPageViewModel() {
            _settingService = App.Current.Resources["OverSettingService"] as OverSettingService;
            InitCommand();
            LoadRes();
        }
        #endregion
    }

}
