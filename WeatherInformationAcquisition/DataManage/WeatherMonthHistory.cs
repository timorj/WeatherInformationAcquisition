using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherInformationAcquisition.Utils;

namespace WeatherInformationAcquisition.DataManage
{
    [Serializable]
    public class WeatherMonthHistory
    {
        private string month;
        private string maxTemp;
        private string minTemp;
        private string rain;

        [PropertyNickName("月份")]
        public string Month { get => month; set => month = value; }
        [PropertyNickName("最高气温")]
        public string MaxTemp { get => maxTemp; set => maxTemp = value; }
        [PropertyNickName("最低气温")]
        public string MinTemp { get => minTemp; set => minTemp = value; }
        [PropertyNickName("降雨量")]
        public string Rain { get => rain; set => rain = value; }
        public WeatherMonthHistory()
        {

        }
        public override string ToString()
        {
            return ClassTools.ClassToString(this);
        }
    }
}
