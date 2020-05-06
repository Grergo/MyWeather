using BackgroundTask;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Weather
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public static ApplicationTheme apptheme;
        public App()
        {

           
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            apptheme = this.RequestedTheme;
            if (localSettings.Values["theme"] == null)
            {
                localSettings.Values["theme"] = "light";
                this.RequestedTheme = ApplicationTheme.Light;
            }
            else if (localSettings.Values["theme"].ToString() == "light")
            {
                this.RequestedTheme = ApplicationTheme.Light;
            }
            else if (localSettings.Values["theme"].ToString() == "night")
            {
                this.RequestedTheme = ApplicationTheme.Dark;
            }
            if(localSettings.Values["sound"]==null) {
                localSettings.Values["sound"] = "0";
            }
            else if(localSettings.Values["sound"].ToString()=="0")
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
            }
            else
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
                ElementSoundPlayer.Volume = 1.0;

            }
            this.InitializeComponent();
            this.Suspending += OnSuspending;   
            



        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override  void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
                ExtendAcrylicIntoTitleBar();
                Thread thread = new Thread(Start_work);
                thread.Start();
               
            }
        }
        // 注册通知，获取磁贴
        private async void Start_work()
        {
            try
            {
                PushNotificationChannel pushNotificationChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                pushNotificationChannel.PushNotificationReceived += PushNotificationChannel_PushNotificationReceived;
                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (localSettings.Values["Rain"].ToString() != "0")
                {
                    if (localSettings.Values["noticeurl"] == null)
                    {
                        localSettings.Values["noticeurl"] = pushNotificationChannel.Uri;
                        dic.Add("lable", "new");
                        dic.Add("newurl", Convert.ToBase64String(Encoding.UTF8.GetBytes(pushNotificationChannel.Uri)));
                        dic.Add("oldurl", "*");
                        if (localSettings.Values["notice"].ToString() != "0")
                        {
                            dic.Add("location", localSettings.Values["startLocation"].ToString());
                        }
                        else
                        {
                            dic.Add("location", "*");
                        }

                        Function.HTTP.Post("http://47.93.43.20:8100/UWP/URL", dic);
                    }
                    else
                    {
                        dic.Add("lable", "replace");
                        dic.Add("newurl", Convert.ToBase64String(Encoding.UTF8.GetBytes(pushNotificationChannel.Uri)));
                        dic.Add("oldurl", Convert.ToBase64String(Encoding.UTF8.GetBytes(localSettings.Values["noticeurl"].ToString())));
                        if (localSettings.Values["notice"].ToString() != "0")
                        {
                            dic.Add("location", localSettings.Values["startLocation"].ToString());
                        }
                        else
                        {
                            dic.Add("location", "*");
                        }
                        localSettings.Values["noticeurl"] = pushNotificationChannel.Uri;
                        Function.HTTP.Post("http://47.93.43.20:8100/UWP/URL", dic);
                    }
                }
            }
            catch
            {
                return;
            }
            await RequestTile();
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
        private void ExtendAcrylicIntoTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
        private void PushNotificationChannel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            if (args.NotificationType == PushNotificationType.Toast)
            {
                ToastNotificationManager.CreateToastNotifier().Show(args.ToastNotification);
            }
        }
        private async Task RequestTile()
        {
            Root root = null;
            try
            {   
                root = await GetData();
            }
            catch
            {
                return;
            }
            

            var tileNotif = new TileNotification(CreateCard(root).GetXml());


            // And send the notification to the primary tile
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotif);
        }
        private TileContent CreateCard(Root root)
        {
            var tileContent = new TileContent()
            {
                Visual = new TileVisual()
                {
                    DisplayName = root.location,
                    TileSmall = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            TextStacking = TileTextStacking.Center,
                            Children =
                {
                    new AdaptiveText()
                    {
                        Text =root.today,
                        HintStyle = AdaptiveTextStyle.Body,
                        HintAlign = AdaptiveTextAlign.Center
                    },
                    new AdaptiveText()
                    {
                        Text = root.now_tmp,
                        HintStyle = AdaptiveTextStyle.Base,
                        HintAlign = AdaptiveTextAlign.Center
                    }
                },
                            BackgroundImage = new TileBackgroundImage()
                            {
                                Source = root.backimage
                            }
                        }
                    },
                    TileMedium = new TileBinding()
                    {
                        Branding = TileBranding.Name,
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
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[0].day,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveImage()
                                    {
                                        HintRemoveMargin = true,
                                        Source = root.forecast[0].weathericon
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[0].tmp_max,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[0].tmp_min,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        HintAlign = AdaptiveTextAlign.Center
                                    }
                                }
                            },
                            new AdaptiveSubgroup()
                            {
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[1].day,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveImage()
                                    {
                                        HintRemoveMargin = true,
                                        Source = root.forecast[1].weathericon
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[1].tmp_max,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[1].tmp_min,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        HintAlign = AdaptiveTextAlign.Center
                                    }
                                }
                            }
                        }
                    }
                },
                            BackgroundImage = new TileBackgroundImage()
                            {
                                Source = root.backimage
                            }
                        }
                    },
                    TileWide = new TileBinding()
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
                                HintWeight = 1,
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[0].day,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveImage()
                                    {
                                        HintRemoveMargin = true,
                                        Source = root.forecast[0].weathericon
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[0].tmp_max,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[0].tmp_min,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        HintAlign = AdaptiveTextAlign.Center
                                    }
                                }
                            },
                            new AdaptiveSubgroup()
                            {
                                HintWeight = 1,
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[1].day,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveImage()
                                    {
                                        HintRemoveMargin = true,
                                        Source = root.forecast[1].weathericon
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[1].tmp_max,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[1].tmp_min,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        HintAlign = AdaptiveTextAlign.Center
                                    }
                                }
                            },
                            new AdaptiveSubgroup()
                            {
                                HintWeight = 1,
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[2].day,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveImage()
                                    {
                                        HintRemoveMargin = true,
                                        Source = root.forecast[2].weathericon
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[2].tmp_max,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[2].tmp_min,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        HintAlign = AdaptiveTextAlign.Center
                                    }
                                }
                            },
                            new AdaptiveSubgroup()
                            {
                                HintWeight = 1,
                               Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[3].day,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveImage()
                                    {
                                        HintRemoveMargin = true,
                                        Source = root.forecast[3].weathericon
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[3].tmp_max,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[3].tmp_min,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        HintAlign = AdaptiveTextAlign.Center
                                    }
                                }
                            },
                            new AdaptiveSubgroup()
                            {
                                HintWeight = 1,
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[4].day,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveImage()
                                    {
                                        HintRemoveMargin = true,
                                        Source = root.forecast[4].weathericon
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[4].tmp_max,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[4].tmp_min,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        HintAlign = AdaptiveTextAlign.Center
                                    }
                                }
                            }
                        }
                    }
                },
                            BackgroundImage = new TileBackgroundImage()
                            {
                                Source = root.backimage
                            }
                        }
                    },
                    TileLarge = new TileBinding()
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
                                HintWeight = 30,
                                Children =
                                {
                                    new AdaptiveImage()
                                    {
                                        Source = root.now_weatherIcon
                                    }
                                }
                            },
                            new AdaptiveSubgroup()
                            {
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = root.today,
                                        HintStyle = AdaptiveTextStyle.Base
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.today_tmp
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.today_Rain,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.today_wind,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                    }
                                }
                            }
                        }
                    },
                    new AdaptiveText()
                    {
                        Text = ""
                    },
                    new AdaptiveGroup()
                    {
                        Children =
                        {
                            new AdaptiveSubgroup()
                            {
                                HintWeight = 1,
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[1].day,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveImage()
                                    {
                                        Source = root.forecast[1].weathericon
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[1].tmp_max,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[1].tmp_min,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        HintAlign = AdaptiveTextAlign.Center
                                    }
                                }
                            },
                            new AdaptiveSubgroup()
                            {
                                HintWeight = 1,
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[2].day,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveImage()
                                    {
                                        Source = root.forecast[2].weathericon
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[2].tmp_max,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[2].tmp_min,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        HintAlign = AdaptiveTextAlign.Center
                                    }
                                }
                            },
                            new AdaptiveSubgroup()
                            {
                                HintWeight = 1,
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[3].day,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveImage()
                                    {
                                        Source = root.forecast[3].weathericon
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[3].tmp_max,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[3].tmp_min,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        HintAlign = AdaptiveTextAlign.Center
                                    }
                                }
                            },
                            new AdaptiveSubgroup()
                            {
                                HintWeight = 1,
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[4].day,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveImage()
                                    {
                                        Source = root.forecast[4].weathericon
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[4].tmp_max,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = root.forecast[4].tmp_min,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                        HintAlign = AdaptiveTextAlign.Center
                                    }
                                }
                            }
                        }
                    }
                },
                            BackgroundImage = new TileBackgroundImage()
                            {
                                Source = root.backimage
                            }
                        }
                    }
                }
            };
            return tileContent;
        }
        private async Task<Root> GetData()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string URL = "http://47.93.43.20:8100/UWP/Weather";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("location", localSettings.Values["startLocation"].ToString());
            Root data = JsonConvert.DeserializeObject<Root>(await Get(URL, dic));
            return data;
        }
        private async Task<string> Get(string url, Dictionary<string, string> dic)
        {
            string result = "";
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in dic)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, System.Web.HttpUtility.UrlEncode(item.Value));
                    i++;
                }
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            //添加参数
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
        }
    }
}
