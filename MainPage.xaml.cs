using System.Collections.Generic;
using System.Diagnostics;
using Weather.Views;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Weather
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        public static ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private  string StartLocation="auto_ip";
        public MainPage()
        {
            this.InitializeComponent();
            Init_Config();
            ChangetoForcast(StartLocation);
            Current = this;
        }

        private void WeatherMainView_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(Setting));
            }
            else
            {
                var navItemTag = args.InvokedItemContainer.Tag.ToString();
                switch (navItemTag)
                {
                    case "Forecast":
                        ContentFrame.CacheSize = 2;
                        ContentFrame.Navigate(typeof(Forecast), null, new EntranceNavigationTransitionInfo());
                        break;
                    case "fav":
                        ContentFrame.CacheSize = 2;
                        ContentFrame.Navigate(typeof(Fav), null, new EntranceNavigationTransitionInfo());
                        break;
                }
            }
        }
        private void ChangetoForcast(string location)
        {
            Weather.Views.Forecast.location = StartLocation;
            ContentFrame.Navigate(typeof(Forecast),null, new DrillInNavigationTransitionInfo());
        }
        private void Init_Config()
        {
            if (localSettings.Values["startLocation"] == null)
            {
                localSettings.Values["startLocation"] = "auto_ip";
            }
            else
            {
                StartLocation = localSettings.Values["startLocation"].ToString();
            }
            if (localSettings.Values["favlocation"] == null)
            {
                Windows.Storage.ApplicationDataCompositeValue Weather = new Windows.Storage.ApplicationDataCompositeValue();
                localSettings.Values["favlocation"] = Weather;
            }
            
        }
    }
}
