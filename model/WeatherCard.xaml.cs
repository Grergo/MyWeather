using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Weather.model
{
    public sealed partial class WeatherCard : UserControl
    {
        public WeatherCard(Function.Daily_forecast daily)
        {
            this.InitializeComponent();
            des.Text = daily.cond_txt_d;
            BitmapImage i = new BitmapImage(new Uri(new StringBuilder("ms-appx:///Resource/WeatherIcon/128/").Append(daily.cond_code_d).Append(".png").ToString()));
            Weathericon.Source = i;
            tem_max.Text = daily.tmp_max+"℃";
            tem_min.Text = daily.tmp_min+"℃";
            Date.Text = daily.date.Day.ToString() + "日 ("+Week(daily.date)+")";
        }
        public string Week(DateTime d)
        {
            string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = weekdays[Convert.ToInt32(d.DayOfWeek)];
            return week;
        }
    }
}
