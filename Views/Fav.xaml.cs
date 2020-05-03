using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Weather.model;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

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
            load_startlocation();
            load_favlocation();
            
        }
        public void load_startlocation()
        {
            this.Invoke(()=>
            {
                string location = MainPage.localSettings.Values["startLocation"] as string;
                string Now_URL = "https://free-api.heweather.net/s6/weather/now";
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("location", location);
                dic.Add("key", key);
                try
                {
                    container1.Items.Add(new FavCard(JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(Now_URL, dic))));
                }
                catch
                {
                    container1.Items.Add(new FavCard(JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(Now_URL, dic))));
                }
                MainPage.Current.ContentFrame.CacheSize = 2;
            });
            
            
        }
        public void load_favlocation()
        {
            this.Invoke(() =>
            {
                string Now_URL = "https://free-api.heweather.net/s6/weather/now";
                string location;
                Windows.Storage.ApplicationDataCompositeValue weatherlist = MainPage.localSettings.Values["favlocation"] as ApplicationDataCompositeValue;
                if (weatherlist.Count != 0)
                {
                    for (int i = 0; i < weatherlist.Count; i++)
                    {
                        location = weatherlist[i.ToString()].ToString();
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("location", location);
                        dic.Add("key", key);
                        container2.Items.Add(new FavCard(JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(Now_URL, dic))));
                    }
                }
            });
            
        }
        public async void Invoke(Action action, Windows.UI.Core.CoreDispatcherPriority Priority = Windows.UI.Core.CoreDispatcherPriority.Normal)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Priority, () => { action(); });
        }
    }
}
