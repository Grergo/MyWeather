using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Weather.model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Weather.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Fav : Page
    {
        private string key = "bb44f1efcb554198bcf14ef08f3bcd51";
        public Fav()
        {
            this.InitializeComponent();
            string Now_URL = "https://free-api.heweather.net/s6/weather/now";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("location", "auto_ip");
            dic.Add("key",key );
            container1.Items.Add(new FavCard(JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(Now_URL, dic))));
        }
    }
}
