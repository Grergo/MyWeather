using LiveCharts;
using LiveCharts.Uwp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Weather.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Forecast : Page
    {
        private ObservableCollection<String> suggestions;
        private string key = "bb44f1efcb554cf14ef08f3bcd51";
       // private ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private List<string> Labels { get; set; }
        public static string location { get; set; }
        public SeriesCollection sec { get; set; }
        public Forecast()
        {
            this.InitializeComponent();
            suggestions = new ObservableCollection<string>();
            try
            {
                loadWeather(location);
                load_Dalily(location);
                ShowChart(location);
                load_des(location);
            }
            catch
            {
                return;
            }
        
            

        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (Searchbox.Text != "")
            {
                if (!Searchbox.Text.Contains(","))
                {
                    string URL = "https://search.heweather.net/find";
                    suggestions.Clear();
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("key", key);
                    dic.Add("mode", "match");
                    dic.Add("number", "3");
                    dic.Add("location", Searchbox.Text);
                    JObject jo = (JObject)JsonConvert.DeserializeObject(Function.HTTP.Get(URL, dic));
                    try
                    {
                        if (jo["HeWeather6"][0]["status"].ToString() == "ok")
                        {
                            foreach (var i in jo["HeWeather6"][0]["basic"])
                            {
                                suggestions.Add(i["location"] + "," + i["parent_city"] + "," + i["admin_area"]);
                            }
                            sender.ItemsSource = suggestions;
                        }

                    }
                    catch (Exception)
                    {

                    }
                }
                
                
            }

        }

        private void Searchbox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            
            if (Searchbox.Text.Contains(","))
            {
               location=Searchbox.Text.Split(',')[0];
            }
            else
            {
                location = Searchbox.Text;
            }
            string Now_URL = "https://free-api.heweather.net/s6/weather/now";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("location", location);
            dic.Add("key", key);
            Function.Weather nowWeather = null;
            try
            {
                nowWeather = JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(Now_URL, dic));
            }
            catch (Exception e)
            {
                nowWeather = JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(Now_URL, dic));
            }
            if (nowWeather.HeWeather6[0].status == "ok")
            {
                
                Update_NowWeather(nowWeather);
            }
            ShowChart(location);
            load_Dalily(location);
            load_des(location);

        }
        private void Update_NowWeather(Function.Weather nowWeather)
        {
            Loadbackground(nowWeather);
            City.Text = nowWeather.HeWeather6[0].basic.cnty.Replace("中国","中华人民共和国") + nowWeather.HeWeather6[0].basic.location;
            BitmapImage i = new BitmapImage(new Uri(new StringBuilder("ms-appx:///Resource/WeatherIcon/128/").Append(ChangeNightIcon(nowWeather.HeWeather6[0].now.cond_code)).Append(".png").ToString()));
            Icon.Source = i;
            Temp.Text = nowWeather.HeWeather6[0].now.tmp+"℃";
            des.Text = nowWeather.HeWeather6[0].now.cond_txt;
            tiganTemp.Text = "体感温度  " + nowWeather.HeWeather6[0].now.fl+"℃";
            fx.Rotation= Convert.ToDouble(nowWeather.HeWeather6[0].now.wind_deg)+90;
            fs.Text = nowWeather.HeWeather6[0].now.wind_spd + "公里/小时";
            njd.Text = nowWeather.HeWeather6[0].now.vis + "公里";
            qyj.Text = nowWeather.HeWeather6[0].now.pres + "百帕";
            sd.Text = nowWeather.HeWeather6[0].now.hum + "%";
            yl.Text = nowWeather.HeWeather6[0].now.cloud;
            time.Text="最后更新时间: "+nowWeather.HeWeather6[0].update.loc.ToString("HH:mm");
        }
        private void loadWeather(string location)
        {
            string Now_URL = "https://free-api.heweather.net/s6/weather/now";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("location", location);
            dic.Add("key", key);
            Function.Weather nowWeather = null;
            try
            {
                nowWeather = JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(Now_URL, dic));
            }
            catch(Exception e)
            {
                nowWeather = JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(Now_URL, dic));
            }
            
            if (nowWeather.HeWeather6[0].status == "ok")
            {
                
                Update_NowWeather(nowWeather);

            }
        }
        private string ChangeNightIcon(string icon)
        {
            TimeSpan startSpan = DateTime.Parse("19:30").TimeOfDay;
            TimeSpan endSpan = DateTime.Parse("06:00").TimeOfDay;
            List<string> filelist = new List<string> { "150", "153","154","350","351","457","456" };
            if (DateTime.Now.TimeOfDay>=startSpan)
            {
                if (filelist.Contains((int.Parse(icon)+50).ToString()))
                {
                    return (int.Parse(icon) + 50).ToString();
                }
                else
                {
                    return icon;
                }
            }
            else if(DateTime.Now.TimeOfDay<= endSpan)
            {
                if (filelist.Contains((int.Parse(icon) + 50).ToString()))
                {
                    return (int.Parse(icon) + 50).ToString();
                }
                else
                {
                    return icon;
                }
            }
            else
            {
                return icon;
            }
            

        }

        private void reload_Click(object sender, RoutedEventArgs e)
        {
            loadWeather(location);
            load_Dalily(location);
            ShowChart(location);
            load_des(location);
        }
        private void load_Dalily(string location = "auto_ip")
        {
            this.Invoke(() =>
            {
                string Dalily_URL = "https://free-api.heweather.net/s6/weather/forecast";
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("location", location);
                dic.Add("key", key);
                ListViewRiver.Items.Clear();
                ObservableCollection<model.WeatherCard> Dalily = new ObservableCollection<model.WeatherCard>();
                Function.Weather daylilyWeather = null;
                try
                {
                     daylilyWeather = JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(Dalily_URL, dic));
                }catch(Exception e)
                {
                     daylilyWeather = JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(Dalily_URL, dic));
                }
                
                foreach (Function.Daily_forecast day in daylilyWeather.HeWeather6[0].daily_forecast)
                {
                    ListViewRiver.Items.Add(new Weather.model.WeatherCard(day));
                }
                sunup.Text = daylilyWeather.HeWeather6[0].daily_forecast[0].sr;
                sundown.Text = daylilyWeather.HeWeather6[0].daily_forecast[0].ss;
                moonup.Text = daylilyWeather.HeWeather6[0].daily_forecast[0].mr;
                moondown.Text = daylilyWeather.HeWeather6[0].daily_forecast[0].ms;
            });
            
            
        }
        private void ShowChart(string location="auto_ip")
        {
            this.Invoke(async () =>
            {
                string Dalily_URL = "https://free-api.heweather.net/s6/weather/hourly";
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("location", location);
                dic.Add("key", key);
                if(tmpchart.Series!=null)
                tmpchart.Series.Clear();
                Function.Weather hourlyWeather = null;
                try
                {
                     hourlyWeather = JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(Dalily_URL, dic));
                }catch(Exception e)
                {
                     hourlyWeather = JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(Dalily_URL, dic));
                }
                
                LineSeries mylineseries = new LineSeries();
                LineSeries weatherlines = new LineSeries();
                sec = new SeriesCollection { };
                tmpchart.Series = sec;
                weatherlines.Values = new ChartValues<double>();
                mylineseries.LineSmoothness = 2;
                mylineseries.PointGeometry = DefaultGeometries.None;
                mylineseries.Values = new ChartValues<double>();
                mylineseries.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                mylineseries.FontSize = 14;
                weatherlines.LineSmoothness = 2;
                weatherlines.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                weatherlines.PointGeometry = DefaultGeometries.None;
                weatherlines.Values = new ChartValues<double>();
                weatherlines.FontSize = 14;
                weatherlines.StrokeThickness = 0.5;
                weatherlines.Opacity = 0.2;
                sec.Add(mylineseries);
                sec.Add(weatherlines);
                tmpchart.AxisX[0].Separator = new Separator
                {
                    Step = 1,
                    StrokeThickness = 0,
                };
                tmpchart.AxisY[0].Separator = new Separator
                {
                    Step = 5,
                    StrokeThickness = 0,
                };
                var temlist = new List<double>();
                Labels = new List<string>();
                DataContext = this;
                mylineseries.DataLabels = true;
                List<string> lablepoint = new List<string>();
                foreach (Function.Hourly h in hourlyWeather.HeWeather6[0].hourly)
                {
                    mylineseries.Values.Add(Convert.ToDouble(h.tmp));
                    temlist.Add(Convert.ToDouble(h.tmp));

                    mylineseries.LabelPoint = point => point.Y + "℃";
                    Labels.Add(h.time.ToString("HH:mm") + "\n " + h.pop + "%");
                    lablepoint.Add(h.cond_txt);
                }
                var v = (temlist.Max() + temlist.Min()) / 8;
                weatherlines.DataLabels = true;
                foreach(Function.Hourly h in hourlyWeather.HeWeather6[0].hourly)
                {
                    weatherlines.Values.Add(v);
                    weatherlines.LabelPoint = p =>
                    {

                        return lablepoint[p.Key];

                    };
                    await Task.Delay(180);
                }
                
            });
        }
        public async void Invoke(Action action, Windows.UI.Core.CoreDispatcherPriority Priority = Windows.UI.Core.CoreDispatcherPriority.Normal)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Priority, () => { action(); });
        }
        private void load_des(string location="auto_ip")
        {
            this.Invoke(() =>
            {
                string life_URL = "https://free-api.heweather.net/s6/weather/lifestyle";
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("location", location);
                dic.Add("key", key);
                Function.Weather lifeWeather = null;
                try
                {
                    lifeWeather = JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(life_URL, dic));
                }
                catch (Exception e)
                {
                    lifeWeather = JsonConvert.DeserializeObject<Function.Weather>(Function.HTTP.Get(life_URL, dic));
                }
                comfortable.Text = lifeWeather.HeWeather6[0].lifestyle[0].brf + "，" + lifeWeather.HeWeather6[0].lifestyle[0].txt;
                cloth.Text = lifeWeather.HeWeather6[0].lifestyle[1].txt;
            });
           
        }

        private void Fav_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataCompositeValue weatherlist= MainPage.localSettings.Values["favlocation"] as ApplicationDataCompositeValue;
            weatherlist[weatherlist.Count.ToString()] = location;
            MainPage.localSettings.Values["favlocation"] = weatherlist;
            MainPage.Current.ContentFrame.CacheSize = 0;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            
            MainPage.localSettings.Values["startLocation"] = location;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("noticeurl", MainPage.localSettings.Values["noticeurl"].ToString());
            dic.Add("location", location);
            Function.HTTP.Post("http://192.168.5.2/UWP/INFO", dic);
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

            }else if(DateTime.Now.TimeOfDay >= span2 && DateTime.Now.TimeOfDay <= span3)
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
            else if(DateTime.Now.TimeOfDay >= span3 || DateTime.Now.TimeOfDay <= span1)
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
