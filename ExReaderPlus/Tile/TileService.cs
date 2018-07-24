using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

 /// <summary>
 /// 等待按钮调用
 /// </summary>
namespace ExReaderPlus.Tile
{
    /// <summary>
    /// 动态磁贴
    /// </summary>
    class TileService
    {
        /// <summary>
        /// 固定到开始屏幕
        /// </summary>
        public async void PinTile()
        {

            
            SecondaryTile tile = new SecondaryTile(DateTime.Now.Ticks.ToString())
            {
                DisplayName = "Exreader",
                Arguments = "args"
            };
            tile.VisualElements.Square150x150Logo = new Uri("ms-appx:///Assets/Template1.png");
            tile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Template1.png");
            tile.VisualElements.Square310x310Logo = new Uri("ms-appx:///Assets/Template1.png");
            tile.VisualElements.ShowNameOnSquare150x150Logo = true;
            tile.VisualElements.ShowNameOnSquare310x310Logo = true;
            tile.VisualElements.ShowNameOnWide310x150Logo = true;


            if (!await tile.RequestCreateAsync())
            {
                return;
            }

            // Generate the tile notification content and update the tile
            TileContent content = GenerateTileContent("Friend", "Assets/Square44x44Logo.scale-200.png");
            TileUpdateManager.CreateTileUpdaterForSecondaryTile(tile.TileId).Update(new TileNotification(content.GetXml()));
        }
        /// <summary>
        /// 响应用户改变大小操作
        /// </summary>
        /// <param name="username"></param>
        /// <param name="avatarLogoSource"></param>
        /// <returns></returns>
        public static TileContent GenerateTileContent(string username, string avatarLogoSource)
        {
            return new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = GenerateTileBindingMedium(username, avatarLogoSource),
                    TileWide = GenerateTileBindingWide(username, avatarLogoSource),
                    TileLarge = GenerateTileBindingLarge(username, avatarLogoSource)
                }
            };
        }

        /// <summary>
        /// 中等图标
        /// </summary>
        /// <param name="username"></param>
        /// <param name="avatarLogoSource"></param>
        /// <returns></returns>
        private static TileBinding GenerateTileBindingMedium(string username, string avatarLogoSource)
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    PeekImage = new TilePeekImage()
                    {
                        Source = avatarLogoSource,
                        HintCrop = TilePeekImageCrop.Circle
                    },

                    TextStacking = TileTextStacking.Center,

                    Children =
            {
                new AdaptiveText()
                {
                    Text = "Hi,",
                    HintAlign = AdaptiveTextAlign.Center,
                    HintStyle = AdaptiveTextStyle.Base
                },

                new AdaptiveText()
                {
                    Text = username,
                    HintAlign = AdaptiveTextAlign.Center,
                    HintStyle = AdaptiveTextStyle.CaptionSubtle
                }
            }
                }
            };
        }

        /// <summary>
        /// 宽图标
        /// </summary>
        /// <param name="username"></param>
        /// <param name="avatarLogoSource"></param>
        /// <returns></returns>
        private static TileBinding GenerateTileBindingWide(string username, string avatarLogoSource)
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    Children =
            {
                new AdaptiveGroup()
                {
                    Children =
                    {
                        new AdaptiveSubgroup()
                        {
                            HintWeight = 33,

                            Children =
                            {
                                new AdaptiveImage()
                                {
                                    Source = avatarLogoSource,
                                    HintCrop = AdaptiveImageCrop.Circle
                                }
                            }
                        },

                        new AdaptiveSubgroup()
                        {
                            HintTextStacking = AdaptiveSubgroupTextStacking.Center,

                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = "Hi,",
                                    HintStyle = AdaptiveTextStyle.Title
                                },

                                new AdaptiveText()
                                {
                                    Text = username,
                                    HintStyle = AdaptiveTextStyle.SubtitleSubtle
                                }
                            }
                        }
                    }
                }
            }
                }
            };
        }

        /// <summary>
        /// 大图标
        /// </summary>
        /// <param name="username"></param>
        /// <param name="avatarLogoSource"></param>
        /// <returns></returns>
        private static TileBinding GenerateTileBindingLarge(string username, string avatarLogoSource)
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    TextStacking = TileTextStacking.Center,

                    Children =
            {
                new AdaptiveGroup()
                {
                    Children =
                    {
                        new AdaptiveSubgroup()
                        {
                            HintWeight = 1
                        },

                        // We surround the image by two subgroups so that it doesn't take the full width
                        new AdaptiveSubgroup()
                        {
                            HintWeight = 2,
                            Children =
                            {
                                new AdaptiveImage()
                                {
                                    Source = avatarLogoSource,
                                    HintCrop = AdaptiveImageCrop.Circle
                                }
                            }
                        },

                        new AdaptiveSubgroup()
                        {
                            HintWeight = 1
                        }
                    }
                },

                new AdaptiveText()
                {
                    Text = "Hi,",
                    HintAlign = AdaptiveTextAlign.Center,
                    HintStyle = AdaptiveTextStyle.Title
                },

                new AdaptiveText()
                {
                    Text = username,
                    HintAlign = AdaptiveTextAlign.Center,
                    HintStyle = AdaptiveTextStyle.SubtitleSubtle
                }
            }
                }
            };
        }
    }
}
