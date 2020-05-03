using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
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
    public sealed partial class Setting : Page
    {
        public Setting()
        {
            this.InitializeComponent();
            loadstatus();
        }
        private void loadstatus()
        {
            if (MainPage.localSettings.Values["theme"].ToString() == "night")
            {
                theme.IsOn = true;
            }
            else
            {
                theme.IsOn = false;
            }
            if(MainPage.localSettings.Values["sound"].ToString() == "1")
            {
                sounds.IsOn = true;
            }
            else
            {
                sounds.IsOn = false;
            }
            if(MainPage.localSettings.Values["rain"].ToString() == "1")
            {
                Rain.IsOn = true;
            }
            else
            {
                Rain.IsOn = false;
            }
            if (MainPage.localSettings.Values["notice"].ToString() == "1")
            {
                notice.IsOn = true;
            }
            else
            {
                notice.IsOn =false;
            }

        }

        private  void  theme_Toggled(object sender, RoutedEventArgs e)
        {
            if (theme.IsOn)
            {
                MainPage.localSettings.Values["theme"] = "night";
            }
            else
            {
                MainPage.localSettings.Values["theme"] = "light";
            }
        }

        private void sounds_Toggled(object sender, RoutedEventArgs e)
        {
            if (sounds.IsOn)
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
                ElementSoundPlayer.Volume = 1.0;
                MainPage.localSettings.Values["sound"] = "1";
            }
            else
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                MainPage.localSettings.Values["sound"] ="0";
            }
        }

        private void Rain_Toggled(object sender, RoutedEventArgs e)
        {
            if (Rain.IsOn)
            {
                MainPage.localSettings.Values["rain"] = "1";
            }
            else
            {
                MainPage.localSettings.Values["rain"] = "0";
            }
            
        }

        private void notice_Toggled(object sender, RoutedEventArgs e)
        {
            if (notice.IsOn)
            {
                MainPage.localSettings.Values["notice"] = "1";
            }
            else
            {
                MainPage.localSettings.Values["notice"] = "0";
            }
        }
    }
}
