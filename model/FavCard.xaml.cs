using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Weather.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Weather.model
{
    public sealed partial class FavCard : UserControl
    {
        private string loca;
        public FavCard(Function.Weather weather)
        {
            this.InitializeComponent();
            location.Text=weather.HeWeather6[0].basic.cnty.Replace("中国", "中华人民共和国") + weather.HeWeather6[0].basic.location;
            tmp.Text = weather.HeWeather6[0].now.tmp + "℃";
            des.Text = weather.HeWeather6[0].now.cond_txt;
            fs.Text = weather.HeWeather6[0].now.wind_spd + "公里/小时";
            fx.Rotation= Convert.ToDouble(weather.HeWeather6[0].now.wind_deg) + 90;
            sd.Text = weather.HeWeather6[0].now.hum + "%";
            loca = weather.HeWeather6[0].basic.location;

        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if(loca== MainPage.localSettings.Values["startLocation"].ToString())
            {
                MainPage.localSettings.Values["startLocation"] ="auto_ip";
                Forecast.location = "auto_ip";
            }
            else
            {
                Windows.Storage.ApplicationDataCompositeValue weatherlist = MainPage.localSettings.Values["favlocation"] as ApplicationDataCompositeValue;
                Windows.Storage.ApplicationDataCompositeValue Weather_new = new Windows.Storage.ApplicationDataCompositeValue();
                int j = 0;
                for (int i = 0; i < weatherlist.Count; i++)
                {
                    if (weatherlist[i.ToString()].ToString() != loca)
                    {
                        Weather_new[j.ToString()] = weatherlist[i.ToString()];
                        j++;
                    }
                    else
                    {
                        continue;
                    }
                }
                MainPage.localSettings.Values["favlocation"] = Weather_new;
                
            }
            MainPage.Current.ContentFrame.CacheSize = 0;
            MainPage.Current.ContentFrame.Navigate(typeof(Fav), null, new EntranceNavigationTransitionInfo());

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Forecast.location = loca;
            MainPage.Current.ContentFrame.CacheSize = 0;
            MainPage.Current.ContentFrame.Navigate(typeof(Forecast), null, new DrillInNavigationTransitionInfo());
            MainPage.Current.WeatherMainView.SelectedItem = MainPage.Current.WeatherMainView.MenuItems[0];
        }
    }
}
