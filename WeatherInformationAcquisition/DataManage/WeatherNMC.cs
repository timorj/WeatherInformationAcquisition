using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherInformationAcquisition.Utils;

namespace WeatherInformationAcquisition.DataManage
{
    [Serializable]
    public class WeatherNMC:IWeather
    {
        
        private string province;
        
        private string city;
        
        private string pushTime;

        
        private WeatherDayCondition weatherDay;
        
        private List<WeatherHour> weatherOneDay;

       
        private List<WeatherDay> weather14Days;
        
        private List<WeatherMonthHistory> weatherHistory;

        [PropertyNickName("省份")]
        public string Province { get => province; set => province = value; }
        [PropertyNickName("城市")]
        public string City { get => city; set => city = value; }
        [PropertyNickName("数据时间")]
        public string PushTime { get => pushTime; set => pushTime = value; }

        [PropertyNickName("天气实况")]
        public WeatherDayCondition WeatherDay { get => weatherDay; set => weatherDay = value; }

        [PropertyNickName("24小时天气情况")]
        public List<WeatherHour> WeatherOneDay { get => weatherOneDay; set => weatherOneDay = value; }

        [PropertyNickName("前后7天的天气情况")]
        public List<WeatherDay> Weather14Days { get => weather14Days; set => weather14Days = value; }

        [PropertyNickName("1981-2010年的天气情况")]
        public List<WeatherMonthHistory> WeatherHistory { get => weatherHistory; set => weatherHistory = value; }


        public WeatherNMC()
        {
            weather14Days = new List<WeatherDay>();
            weatherOneDay = new List<WeatherHour>();
            weatherHistory = new List<WeatherMonthHistory>();
        }

    

        public override string ToString()
        {
            return ClassTools.ClassToString(this);
        }

    }
}
