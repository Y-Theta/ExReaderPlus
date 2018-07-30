using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace ExReaderPlus.View {
    public class OverSettingService {
        #region Properties
        public static OverSettingService Instence = new OverSettingService();

        private OverallViewSettings _viewSettings;
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
                default:
                    value = null;
                    break;
            }
        }

        public void InitRes() {
            _viewSettings = new OverallViewSettings();
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
