using ExReaderPlus.Manage;
using ExReaderPlus.Models;
using ExReaderPlus.Tile;
using ExReaderPlus.ViewModels;
using ExReaderPlus.WordsManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;

namespace ExReaderPlus.View {
    public sealed partial class RichWordView : UserControl {
        #region Properties
        private Timer _enSure;

        private EssayPageViewModel _viewModel;

        private Rect _controlbararea;

        private Rect _nextpagearea;

        private Rect _lastpagearea;

        private Rect _wordlistarea;

        private Point _rectpoint;

        private Point _oripoint;

        private bool _controlbarmove;


        private Dictionary<string, List<Control>> _controlDic;
        public Dictionary<string, List<Control>> ControlDic {
            get => _controlDic;
            set => _controlDic = value;
        }

        /// <summary>
        /// 目标词列表裁剪矩形
        /// </summary>
        public Rect WordPanelRect {
            get { return (Rect)GetValue(WordPanelRectProperty); }
            set { SetValue(WordPanelRectProperty, value); }
        }
        public static readonly DependencyProperty WordPanelRectProperty =
            DependencyProperty.Register("WordPanelRect", typeof(Rect),
                typeof(RichWordView), new PropertyMetadata(null));
        #endregion

        #region Methods

        #region Overrides
        protected override void OnTapped(TappedRoutedEventArgs e) {
            base.OnTapped(e);
            if (_controlbararea.Contains(e.GetPosition(this)) && TextView.IsReadOnly)
                ControlLayer.Visibility = ControlLayer.Visibility.Equals(Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            else if (_lastpagearea.Contains(e.GetPosition(this)) && TextView.IsReadOnly)
                TextView.PageDown();
            else if (_nextpagearea.Contains(e.GetPosition(this)) && TextView.IsReadOnly)
                TextView.PageUp();
            else if (_wordlistarea.Contains(e.GetPosition(this)) && TextView.IsReadOnly)
                WordPanelSwitch();
        }
        #endregion

        #region Eventhandel
        private void RichWordView_Unloaded(object sender, RoutedEventArgs e) {
            _viewModel.PassageLoaded -= EssayPage_PassageLoaded;
            _viewModel.ControlCommand -= _viewModel_ControlCommand;
            _viewModel.WordStateChanged -= _viewModel_WordStateChanged;
            _viewModel.ShownStateChanged -= _viewModel_ShownStateChanged;
            _viewModel.OnRenderChange -= _viewModel_OnRenderChange;
        }

        private void RichWordView_Loaded(object sender, RoutedEventArgs e) {
            _viewModel = (EssayPageViewModel)DataContext;
            _viewModel.PassageLoaded += EssayPage_PassageLoaded;
            _viewModel.ControlCommand += _viewModel_ControlCommand;
            _viewModel.WordStateChanged += _viewModel_WordStateChanged;
            _viewModel.ShownStateChanged += _viewModel_ShownStateChanged;
            _viewModel.OnRenderChange += _viewModel_OnRenderChange;
            TextView.ElementSorted += TextView_ElementSorted;
            TextView.RenderBegin += TextView_RenderBegin;
            ControlLayer.PointerEntered += GridBg_PointerEntered;
            ControlLayer.PointerExited += ControlLayer_PointerExited;
            TextView.IsReadOnly = _viewModel.ReadOnlyState;
        }

        private void _viewModel_OnRenderChange(object sender, string name, bool value) {
            if (name.Equals("Lea"))
            {
                if (value)
                    ShowText(1);
                else
                    HideText(1);
            }
            else
            {
                if (value)
                    ShowText(0);
                else
                    HideText(0);
            }
        }

        private void _viewModel_ShownStateChanged(object sender) {
            _viewModel.LoadKeywordList(Avb_RemCommandAction, PointedItem, ReSetPointedItem);
        }

        private void _viewModel_WordStateChanged(object sender) {
            ActionVocabulary avb = sender as ActionVocabulary;
            ChangControlColor(avb, avb.YesorNo == 0 ? _viewModel.NotLearnBg : _viewModel.LearnedBg);
        }

        /// <summary>
        /// 按钮命令回调
        /// </summary>
        private void _viewModel_ControlCommand(object sender, CommandArgs args) {
            switch (args.parameter)
            {
                case "TurnPageNext": TextView.PageUp(); break;
                case "TurnPageBack": TextView.PageDown(); break;
                case "SizeTextLarge": TextView.FontSize += 0.5; break;
                case "SizeTextLittle": TextView.FontSize -= 0.5; break;
                case "OpenWordList": WordPanelSwitch(); break;
                case "AddToDic": MenuPop.Hide(); break;
                case "Share": Share.Visibility = Share.Visibility.Equals(Visibility.Visible) ? Visibility.Collapsed:Visibility.Visible; break;
                case "ChangeMode":
                    if (TextView.ContentString != null)
                        if (!TextView.IsReadOnly)
                        {
                            TextView.IsReadOnly = true;
                            _viewModel.ReadOnlyState = true;
                        }
                        else
                        {
                            TextView.IsReadOnly = false;
                            _viewModel.ReadOnlyState = false;
                            RenderLayer.Visibility = Visibility.Collapsed;
                        }
                    break;
            }
        }

        private void RichWordView_SizeChanged(object sender, SizeChangedEventArgs e) {
            TextView.ViewPortHeight = e.NewSize.Height
                - TextScroll.Margin.Bottom - TextScroll.Margin.Top - TextScroll.Padding.Bottom - TextScroll.Padding.Top - 24;
            ArrangeRect();
        }

        private void ArrangeRect() {
            _controlbararea = new Rect(TextScroll.ActualWidth / 3, TextScroll.ActualHeight / 3, TextScroll.ActualWidth / 3, TextScroll.ActualHeight / 3);
            _nextpagearea = new Rect(TextScroll.ActualWidth * 5 / 6, 0, TextScroll.ActualWidth / 6, TextScroll.ActualHeight);
            _lastpagearea = new Rect(0, 0, TextScroll.ActualWidth / 6, TextScroll.ActualHeight);
            _wordlistarea = new Rect(TextScroll.ActualWidth / 3, 0, TextScroll.ActualWidth / 3, TextScroll.ActualHeight / 3);
            WordPanelRect = new Rect(0, 0, 280, ActualHeight);
        }

        /// <summary>
        /// 文本处理开始回调
        /// </summary>
        private void TextView_RenderBegin(object sender, EventArgs e) {
            RenderLayer.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 控制面板控制
        /// </summary>
        private void ControlLayer_PointerExited(object sender, PointerRoutedEventArgs e) {
            (sender as Grid).Opacity = 0.3;
        }

        private void GridBg_PointerEntered(object sender, PointerRoutedEventArgs e) {
            (sender as Grid).Opacity = 1;
        }

        /// <summary>
        /// 富文本框的字典构建完毕,转换到阅读模式
        /// </summary>
        private void TextView_ElementSorted(object sender, EventArgs e) {
            _viewModel.ClearKeyWordHashSet();
            _viewModel.WordCount = TextView.WordCount.ToString();
            _viewModel.PageInfo = string.Format("{0}  /  {1}", TextView.TempPage + 1, TextView.SumPage);
            ControlDic.Clear();
            RenderLayer.Children.Clear();
            RenderLayer.UpdateLayout();
            if (TextView.ElementsLoc != null && TextView.ElementsLoc.Count > 0)
                foreach (var kp in TextView.ElementsLoc)
                    foreach (var loc in kp.Value)
                    {
                        HitHolder rect = new HitHolder
                        {
                            PointBrush = _viewModel.NormalBg,
                            Margin = new Thickness(loc.Left - 1, loc.Top + 4, 0, 0),
                            Width = loc.Width + 2,
                            Height = loc.Height - 8,
                            Name = kp.Key,
                        };
                        rect.MouseRightTap += Rect_MouseRightTap;
                        if (WordBook.GetDicNow().Wordlist.ContainsKey(kp.Key))
                        {
                            Vocabulary vc = WordBook.GetDicNow().Wordlist[kp.Key];
                            if (vc.YesorNo == 0)
                            {
                                RenderWithCheck(rect, 0);
                                _viewModel.KeyWordNotLearn.Add(kp.Key);
                            }
                            else
                            {
                                RenderWithCheck(rect, 1);
                                _viewModel.KeyWordLearn.Add(kp.Key);
                            }
                        }
                        rect.PointerEntered += Rect_PointerEntered;
                        AddtoControlDic(kp.Key, rect);
                        RenderLayer.Children.Add(rect);
                    }
            RenderLayer.UpdateLayout();
            TextView.IsEnabled = true;
            if (TextView.IsReadOnly)
            {
                RenderLayer.Visibility = Visibility.Visible;
                if (WordPanel.Visibility.Equals(Visibility.Visible))
                    _viewModel.LoadKeywordList(Avb_RemCommandAction, PointedItem, ReSetPointedItem);
            }
        }

        private void Rect_MouseRightTap(object sender) {
            MenuPop.ShowAt(sender as HitHolder);
        }

        private void PointedItem(object sender) {
            ActionVocabulary s = (ActionVocabulary)sender;
            ChangControlColor(s, _viewModel.NormalBg);
        }

        private void ReSetPointedItem(object sender) {
            ActionVocabulary s = (ActionVocabulary)sender;
            foreach (var hithold in ControlDic[s.Word])
                RenderWithCheck((HitHolder)hithold, s.YesorNo);
        }

        private void Avb_RemCommandAction(object sender, CommandArgs args) {
            ActionVocabulary s = (ActionVocabulary)sender;
            foreach (var hithold in ControlDic[s.Word])
                RenderWithCheck((HitHolder)hithold, s.YesorNo == 1 ? 0 : 1);
            if (s.YesorNo == 0)
            {
                _viewModel.KeyWordNotLearn.Remove(s.Word);
                _viewModel.KeyWordLearn.Add(s.Word);
                _viewModel.UpdateKeywordList(s);
            }
            else
            {
                _viewModel.KeyWordLearn.Remove(s.Word);
                _viewModel.KeyWordNotLearn.Add(s.Word);
                _viewModel.UpdateKeywordList(s);
            }
        }

        private void Rect_PointerEntered(object sender, PointerRoutedEventArgs e) {
            var sb = sender as HitHolder;
            var v1 = fileDatabaseManage.instance.SearchVocabulary(sb.Name.ToLower());
            if (v1 != null)
                sb.Tooltip = v1.Translation.Replace(@"\n", "\n");
        }

        private void AddtoControlDic(string key, Control value) {
            if (ControlDic.ContainsKey(key))
                ControlDic[key].Add(value);
            else
                ControlDic.Add(key, new List<Control>() { value });
        }

        private async void EssayPage_PassageLoaded(object sender, EventArgs e) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TextView.ContentString = (sender as EssayPageViewModel).TempPassage.Content;
            });
        }

        #endregion

        #region PrivateMethods
        public void SetText(string str) {
            TextView.ContentString = str;
        }

        private void ShowText(int learned) {
            if (learned == 1)
            {
                foreach (var s in _viewModel.KeyWordLearn)
                    foreach (var hitc in ControlDic[s])
                        ((HitHolder)hitc).Background = _viewModel.LearnedBg;
            }
            else
                foreach (var s in _viewModel.KeyWordNotLearn)
                    foreach (var hitc in ControlDic[s])
                        ((HitHolder)hitc).Background = _viewModel.NotLearnBg;
        }

        private void HideText(int learned) {
            if (learned == 1)
            {
                foreach (var s in _viewModel.KeyWordLearn)
                    foreach (var hitc in ControlDic[s])
                        ((HitHolder)hitc).Background = new SolidColorBrush(Colors.Transparent);
            }
            else
                foreach (var s in _viewModel.KeyWordNotLearn)
                    foreach (var hitc in ControlDic[s])
                        ((HitHolder)hitc).Background = new SolidColorBrush(Colors.Transparent);
        }

        private void ChangControlColor(ActionVocabulary avb, Brush color) {
            if (color.Equals(_viewModel.NotLearnBg) || color.Equals(_viewModel.LearnedBg))
                foreach (var hithold in ControlDic[avb.Word])
                    RenderWithCheck((HitHolder)hithold, avb.YesorNo);
            else
                foreach (var hithold in ControlDic[avb.Word])
                    ((HitHolder)hithold).Background = color;
        }

        private void RenderWithCheck(HitHolder hit, int learn) {
            if (learn == 1)
            {
                if (_viewModel.LearnedColor)
                    hit.Background = _viewModel.LearnedBg;
                else
                    hit.Background = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                if (_viewModel.NotlearnColor)
                    hit.Background = _viewModel.NotLearnBg;
                else
                    hit.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void WordPanelSwitch() {
            if (WordPanel.Visibility.Equals(Visibility.Visible))
            {
                VisualStateManager.GoToState(this, "WordPanelCollapsed", true);
                _viewModel.SetStateBarButtonFg(Color.FromArgb(255, 8, 8, 8));
            }
            else
            {
                VisualStateManager.GoToState(this, "WordPanelShow", true);
                _viewModel.SetStateBarButtonFg(Color.FromArgb(255, 225, 225, 225));
            }
        }

        private void WordPanelState_CurrentStateChanged(object sender, VisualStateChangedEventArgs e) {
            TextView.FreshLayout();
            ArrangeRect();
        }

        private void InitVisualStates() {
            VisualStateManager.GoToState(this, "WordPanelCollapsed", false);
        }

        private void AttachMethods() {
            SizeChanged += RichWordView_SizeChanged;
            Loaded += RichWordView_Loaded;
            Unloaded += RichWordView_Unloaded;
        }

        private void InitCollections() {
            ControlDic = new Dictionary<string, List<Control>>();
        }

        private void InitTimer() {
            _enSure = new Timer { Interval = 1000 };
            _enSure.Elapsed += _enSure_Elapsed;
        }

        private void _enSure_Elapsed(object sender, ElapsedEventArgs e) {
            _enSure.Enabled = false;
        }

        #endregion

        #region draganddrop
        private void ControlLayer_DragStarting(UIElement sender, DragStartingEventArgs args) {
            _controlbarmove = true;
            _oripoint = args.GetPosition(this);
        }

        private void ControlLayer_DropCompleted(UIElement sender, DropCompletedEventArgs args) {
            _controlbarmove = false;
            Thickness tic = ControlLayer.Margin;
            tic.Bottom = tic.Bottom + _oripoint.Y - _rectpoint.Y < 0 ? 0 : tic.Bottom + _oripoint.Y - _rectpoint.Y;
            ControlLayer.Margin = tic;
        }

        private void Rootgrid_DragOver(object sender, DragEventArgs e) {
            e.DragUIOverride.IsGlyphVisible = false;
        }

        private void Rootgrid_DragLeave(object sender, DragEventArgs e) {
            if (_controlbarmove)
            {
                _rectpoint = e.GetPosition(this);
                if (_rectpoint.Y < 0)
                    _rectpoint = _oripoint;
            }
        }
        #endregion

        #endregion

        #region Constructor
        public RichWordView() {
            InitTimer();
            InitCollections();
            AttachMethods();
            InitializeComponent();
            InitVisualStates();
        }
        #endregion

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {

            //            FileManage.FileManage fileManage= new FileManage.FileManage();
            //            fileManage.NewPage();
        }

        private async void Choose_OnClick(object sender, RoutedEventArgs e)
        {

            string str =
                "Uyghurs in Xinjiang and elsewhere in China face systematic persecution. \r\nWith Islam a fundamental part of the Uyghur identity, so-called counter-terrorism campaigns which have cracked down hard on Muslim practices and increasingly criminalized Islam, are tantamount to the criminalization of being Uyghur.\r\nThis has reportedly included the banning dozens of Uyghur names, with violators at risk of not having their children\'s births registered; to forcing Uyghurs to denounce core tenets of their religion. Parents caught teaching their children about Islam risk detention or having their offspring taken away. \r\nAccording to new research by New York-based Chinese Human Rights Defenders, in 2017 criminal arrests in Xinjiang accounted for a shocking 21% of all national arrests, even though the region\'s population is only 1.5% of China\'s total. In prison, according to state media, so-called \"religious extremists\" euphemistically undergo thought rectification.\r\nAs in apartheid South Africa, checkpoints and restrictions on movement are a fact of daily life for Uyghurs. \r\nArmed police scan IDs, checking biometric and personal data. Religiosity, having relatives abroad, or simply being Uyghur increase the chances of being detained, as do the contents of a person\'s phone or computer. \r\nSince 2015, as I have reported for the London-based Minority Rights Group, Uyghurs have had to obtain permission to visit relatives or seek medical treatment outside their hometowns, and passports started being recalled. Increasingly, the threat of detention and concerns over surveillance make contact between distant family members impossible. \r\nNowhere are the signs of crimes against humanity more alarming than in the expanding system of concentration camps which observers have said are springing up across Xinjiang. \r\nWhile the government officially denies the camps exist, in July state media reported authorities admitted to having transferred some 460,000 Uyghurs for \"vocational training,\" as part of a bid to \"to improve social stability and alleviate poverty.\" \r\nEvidence suggests as many as one million Uyghurs and other Muslims are interned across Xinjiang in \"re-education centers,\" around 10% of the population. \r\nWhat little is known about what happens inside points to systematic physical and psychological torture, and indoctrination. One former detainee said she was not allowed to wear underwear and her head was shaved, while another described having tried to commit suicide by bashing his skull against a wall. Many simply disappear. \r\nMeanwhile, so-called \"becoming family\" and \"home stay\" policies force Uyghur families to accept Communist Party officials into their homes to observe and report on their behavior. Imagine a family member has been taken away and now you are forced to host their abductor, quite possibly in the same room left empty by their disappearance.\r\nChildren whose parents have been detained are transferred to state custody, where, by some accounts, \"they are locked up like farm animals\" in so-called orphanages.";
            var pick = new FileOpenPicker();
            pick.FileTypeFilter.Add(".jpg");
            pick.FileTypeFilter.Add(".png");

            var file = await pick.PickSingleFileAsync();
            var duvDbecdgiu =
                await CanvasBitmap.LoadAsync(new CanvasDevice(true), await file.OpenAsync(FileAccessMode.Read));
            var canvasRenderTarget = new CanvasRenderTarget(duvDbecdgiu, duvDbecdgiu.Size);
            var str1 = str.Substring(0, str.Length / 2);
            char c = str1[str1.Length - 1];
            if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z')
            {
                str1 += str1 + "-";
            }
            else
            {

            }
            var str2 = str.Substring(str.Length / 2);
            //            char[]strLast = str1.Substring(str1.Length).ToCharArray();


            using (var dc = canvasRenderTarget.CreateDrawingSession())//用后则需撤销
            {

                ///先将图片读取
                dc.DrawImage(duvDbecdgiu);
                ///写图片
                dc.DrawText("Total Words :",
                    760, 278, 330, 60,
                    Colors.Blue, new CanvasTextFormat()
                    {
                        FontSize = 30


                    });
                dc.DrawText("Time To Read :",
                    760, 338, 330, 60,
                    Colors.Blue, new CanvasTextFormat()
                    {
                        FontSize = 30



                    });

                var strs1 = str1.Replace("\n", "\n\n");
                var strs2 = str2.Replace("\n", "\n\n");
                dc.DrawText(strs1, 26, 408, 538, 1080, Colors.Black, new CanvasTextFormat()
                {
                    FontSize = 24
                });
                dc.DrawText(strs2, 626, 458, 544, 1080, Colors.Black, new CanvasTextFormat()
                {
                    FontSize = 24
                });
            }
            string desiredName = "Share" + ".png";
            StorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
            //            Debug.WriteLine(applicationFolder.Path);

            StorageFile saveFile = await applicationFolder.CreateFileAsync(desiredName, CreationCollisionOption.GenerateUniqueName);
            await canvasRenderTarget.SaveAsync(await saveFile.OpenAsync(FileAccessMode.ReadWrite), CanvasBitmapFileFormat.Png);
            // 把图片展现出来
            var fileStream = await saveFile.OpenReadAsync();
            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(fileStream);

            Image.Source = bitmap;
        }
    }
}