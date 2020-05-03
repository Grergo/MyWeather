using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Function
{
   public  class Basic
    {
        public string cid { get; set; }
        public string location { get; set; }
        public string parent_city { get; set; }
        public string admin_area { get; set; }
        public string cnty { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string tz { get; set; }
    }

    public  class Update
    {
        public DateTime loc { get; set; }
        public DateTime utc { get; set; }
    }

     public class Now
    {
        public string cloud { get; set; }
        public string cond_code { get; set; }
        public string cond_txt { get; set; }
        public string fl { get; set; }
        public string hum { get; set; }
        public string pcpn { get; set; }
        public string pres { get; set; }
        public string tmp { get; set; }
        public string vis { get; set; }
        public string wind_deg { get; set; }
        public string wind_dir { get; set; }
        public string wind_sc { get; set; }
        public string wind_spd { get; set; }
    }
    public class Daily_forecast
    {
        /// <summary>
        /// 
        /// </summary>
        public string cond_code_d { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cond_code_n { get; set; }
        /// <summary>
        /// 晴
        /// </summary>
        public string cond_txt_d { get; set; }
        /// <summary>
        /// 晴
        /// </summary>
        public string cond_txt_n { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ms { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pcpn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pop { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pres { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ss { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tmp_max { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tmp_min { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uv_index { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vis { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wind_deg { get; set; }
        /// <summary>
        /// 南风
        /// </summary>
        public string wind_dir { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wind_sc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wind_spd { get; set; }
    }
    public class Hourly
    {
        /// <summary>
        /// 
        /// </summary>
        public string cloud { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cond_code { get; set; }
        /// <summary>
        /// 晴
        /// </summary>
        public string cond_txt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dew { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pop { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pres { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tmp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wind_deg { get; set; }
        /// <summary>
        /// 南风
        /// </summary>
        public string wind_dir { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wind_sc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wind_spd { get; set; }
    }
    public class Lifestyle
    {
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 较不舒适
        /// </summary>
        public string brf { get; set; }
        /// <summary>
        /// 白天天气晴好，明媚的阳光在给您带来好心情的同时，也会使您感到有些热，不很舒适。
        /// </summary>
        public string txt { get; set; }
    }


    public class HeWeather6
    {
        public Basic basic { get; set; }
        public Update update { get; set; }
        public string status { get; set; }
        public Now now { get; set; }
        public List<Daily_forecast> daily_forecast { get; set; }
        public List<Hourly> hourly { get; set; }
        public List<Lifestyle> lifestyle { get; set; }
    }

     public class Weather
    {
        public List<HeWeather6> HeWeather6 { get; set; }
    }

}
