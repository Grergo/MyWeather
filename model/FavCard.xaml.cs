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
using Windows.UI.Xaml.Media.Imaging;
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
            Loadbackground(weather);
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
        private void Loadbackground(Function.Weather weather)
        {
            TimeSpan span3 = DateTime.Parse("19:30").TimeOfDay;
            TimeSpan span2 = DateTime.Parse("14:30").TimeOfDay;
            TimeSpan span1 = DateTime.Parse("06:00").TimeOfDay;
            if (DateTime.Now.TimeOfDay >= span1 && DateTime.Now.TimeOfDay <= span2)
            {
                if (weather.HeWeather6[0].now.cond_txt.Contains("雨"))
                {
                    BitmapImage i = new BitmapImage(new Uri("ms-appx:///Resource/background/201.jpg"));
                    background.Source = i;
                }
                else if (weather.HeWeather6[0].now.cond_txt.Contains("雪"))
                {
                    BitmapImage i = new BitmapImage(new Uri("ms-appx:///Resource/background/301.jpg"));
                    background.Source = i;
                }
                else
                {
                    BitmapImage i = new BitmapImage(new Uri("ms-appx:///Resource/background/101.jpg"));
                    background.Source = i;
                }

            }
            else if (DateTime.Now.TimeOfDay >= span2 && DateTime.Now.TimeOfDay <= span3)
            {
                if (weather.HeWeather6[0].now.cond_txt.Contains("雨"))
                {
                    BitmapImage i = new BitmapImage(new Uri("ms-appx:///Resource/background/202.jpg"));
                    background.Source = i;
                }
                else if (weather.HeWeather6[0].now.cond_txt.Contains("雪"))
                {
                    BitmapImage i = new BitmapImage(new Uri("ms-appx:///Resource/background/302.jpg"));
                    background.Source = i;
                }
                else
                {
                    BitmapImage i = new BitmapImage(new Uri("ms-appx:///Resource/background/102.jpg"));
                    background.Source = i;
                }
            }
            else if (DateTime.Now.TimeOfDay >= span3 || DateTime.Now.TimeOfDay <= span1)
            {
                if (weather.HeWeather6[0].now.cond_txt.Contains("雨"))
                {
                    BitmapImage i = new BitmapImage(new Uri("ms-appx:///Resource/background/203.jpg"));
                    background.Source = i;
                }
                else if (weather.HeWeather6[0].now.cond_txt.Contains("雪"))
                {
                    BitmapImage i = new BitmapImage(new Uri("ms-appx:///Resource/background/303.jpg"));
                    background.Source = i;
                }
                else
                {
                    BitmapImage i = new BitmapImage(new Uri("ms-appx:///Resource/background/103.jpg"));
                    background.Source = i;
                }
            }
        }
    }
}
