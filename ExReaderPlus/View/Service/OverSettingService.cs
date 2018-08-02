using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ExReaderPlus.WordsManager;
using System.Diagnostics;

namespace ExReaderPlus.View {
    public class OverSettingService {
        #region Properties
        public static OverSettingService Instence = new OverSettingService();

        private OverallViewSettings _viewSettings;

        private bool SaveComplete = false;
        #endregion

        #region Methods
        public object GetValue(ViewSettingConfigs cofgs) {
            switch (cofgs)
            {
                case ViewSettingConfigs.RichTextBoxBg:
                    return _viewSettings.RichTextBoxBg;
                case ViewSettingConfigs.RichTextBoxFg:
                    return _viewSettings.RichTextBoxFg;
                case ViewSettingConfigs.RichTextBoxSize:
                    return _viewSettings.RichTextBoxSize;
                case ViewSettingConfigs.RichTextBoxWeight:
                    return _viewSettings.RichTextBoxWeight;
                case ViewSettingConfigs.RichTextLearned:
                    return _viewSettings.RichTextLearned;
                case ViewSettingConfigs.RichTextNotLearn:
                    return _viewSettings.RichTextNotLearn;
                case ViewSettingConfigs.RichTextSelectBoxBg:
                    return _viewSettings.RichTextSelectBoxBg;
                case ViewSettingConfigs.IsRenderOn:
                    return _viewSettings.IsRenderOn;
                case ViewSettingConfigs.IsLearnedRender:
                    return _viewSettings.IsLearnedRender;
                case ViewSettingConfigs.IsNotlearnRender:
                    return _viewSettings.IsNotlearnRender;
                case ViewSettingConfigs.ReadingPageControlBar:
                    return _viewSettings.ReadingPageControlBar;
                case ViewSettingConfigs.RichTextBoxLineSpace:
                    return _viewSettings.RichTextBoxLineSpace;
                case ViewSettingConfigs.AppThemeMode:
                    return _viewSettings.AppThemeMode;
                case ViewSettingConfigs.SelectedDic:
                    return _viewSettings.SelectedDic;
                default:return null;
            }
        }

        public void SetValue(ViewSettingConfigs cofgs ,object value) {
            switch (cofgs)
            {
                case ViewSettingConfigs.RichTextBoxBg:
                    _viewSettings.RichTextBoxBg = (Color)value;
                    break;
                case ViewSettingConfigs.RichTextBoxFg:
                    _viewSettings.RichTextBoxFg = (Color)value;
                    break;
                case ViewSettingConfigs.RichTextBoxSize:
                    _viewSettings.RichTextBoxSize = (float)value;
                    break;
                case ViewSettingConfigs.RichTextBoxWeight:
                    _viewSettings.RichTextBoxWeight = (int)value;
                    break;
                case ViewSettingConfigs.RichTextLearned:
                    _viewSettings.RichTextLearned = (Color)value;
                    break;
                case ViewSettingConfigs.RichTextNotLearn:
                    _viewSettings.RichTextNotLearn = (Color)value;
                    break;
                case ViewSettingConfigs.RichTextSelectBoxBg:
                    _viewSettings.RichTextSelectBoxBg = (Color)value;
                    break;
                case ViewSettingConfigs.IsRenderOn:
                    _viewSettings.IsRenderOn = (bool)value;
                    break;
                case ViewSettingConfigs.IsLearnedRender:
                    _viewSettings.IsLearnedRender = (bool)value;
                    break;
                case ViewSettingConfigs.IsNotlearnRender:
                    _viewSettings.IsNotlearnRender = (bool)value;
                    break;
                case ViewSettingConfigs.ReadingPageControlBar:
                    _viewSettings.ReadingPageControlBar = (Thickness)value;
                    break;
                case ViewSettingConfigs.RichTextBoxLineSpace:
                    _viewSettings.RichTextBoxLineSpace = (float)value;
                    break;
                case ViewSettingConfigs.AppThemeMode:
                    _viewSettings.AppThemeMode = (bool)value;
                    break;
                case ViewSettingConfigs.SelectedDic:
                    _viewSettings.SelectedDic = (int)value;
                    break;
                default:
                    value = null;
                    break;
            }
        }

        public bool IsSettingSaved() {
            return SaveComplete;
        }

        private async void InitRes() {
            _viewSettings = await GetLocalSettings();
            if (_viewSettings is null)
                _viewSettings = new OverallViewSettings();
            (Window.Current.Content as Frame).RequestedTheme = _viewSettings.AppThemeMode ? ElementTheme.Light : ElementTheme.Dark;
            WordBook.SelectedDic = _viewSettings.SelectedDic;
        }



        /// <summary>
        /// 读取设置
        /// </summary>
        /// <returns></returns>
        private async Task<OverallViewSettings> GetLocalSettings() {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile f = await folder.TryGetItemAsync("Setting.dat") as StorageFile;
            if (f != null)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(f.CreateSafeFileHandle(), FileAccess.Read))
                {
                    try
                    {
                        if (stream.CanRead && stream.Length != 0)
                        {
                            OverallViewSettings settings = (OverallViewSettings)binaryFormatter.Deserialize(stream);
                            settings.StreamDeCode();
                            return settings;
                        }
                        else
                            return null;
                    }
                    catch { return null; }
                }
            }
            return null;
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <returns></returns>
        public async Task SetLocalSettingsAsync() {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile f = await folder.CreateFileAsync("Setting.dat", CreationCollisionOption.ReplaceExisting);
            using (FileStream stream = new FileStream(f.CreateSafeFileHandle(), FileAccess.Write))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                _viewSettings.StreamCode();
                binaryFormatter.Serialize(stream, _viewSettings);
            }
            SaveComplete = true;
        }

        public void SetStateBarButtonFg(Color color) {
            var TitleBar = ApplicationView.GetForCurrentView().TitleBar;
            TitleBar.ButtonForegroundColor = color;
        }
        #endregion

        #region Constructors
        public OverSettingService() {
            InitRes();
        }
        #endregion
    }

}
