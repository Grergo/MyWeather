using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

// Added during quickstart
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.Web.Syndication;

namespace BackgroundTask
{
    public sealed class WeatherBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely
            // while asynchronous code is still running.
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            // Create the tile notification

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
            deferral.Complete();
        }

        private TileContent CreateCard(Root root)
        {
            var tileContent = new TileContent()
            {
                Visual = new TileVisual()
                {
                    DisplayName=root.location,
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
            string URL = "http://weather.icansudo.top:8100/UWP/Weather";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("location", localSettings.Values["startLocation"].ToString());
            Root data = JsonConvert.DeserializeObject<Root>(await Get(URL,dic));
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
    // 自定义天气类
    
}