using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Weather.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public FavCard(Function.Weather weather)
        {
            this.InitializeComponent();
            location.Text=weather.HeWeather6[0].basic.cnty.Replace("中国", "中华人民共和国") + weather.HeWeather6[0].basic.location;
            tmp.Text = weather.HeWeather6[0].now.tmp + "℃";
            des.Text = weather.HeWeather6[0].now.cond_txt;
            fs.Text = weather.HeWeather6[0].now.wind_spd + "公里/小时";
            fx.Rotation= Convert.ToDouble(weather.HeWeather6[0].now.wind_deg) + 90;
            sd.Text = weather.HeWeather6[0].now.hum + "%";

        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            MainPage.Current.ContentFrame.Navigate(typeof(Forecast), null, new DrillInNavigationTransitionInfo());
            MainPage.Current.WeatherMainView.SelectedItem = MainPage.Current.WeatherMainView.MenuItems[0];
        }
    }
}
