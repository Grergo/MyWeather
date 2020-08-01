using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTask
{
    public sealed class Forecast
    {
        /// <summary>
        /// 
        /// </summary>
        public string day { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string weathericon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tmp_max { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tmp_min { get; set; }
    }

    public sealed class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string backimage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string today { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string now_tmp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string now_weatherIcon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string today_maxtmp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string today_mintmp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<Forecast> forecast { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string today_tmp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string today_Rain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string today_wind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string location { get; set; }
    }
}
