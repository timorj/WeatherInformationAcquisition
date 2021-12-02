using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherInformationAcquisition.Utils;

namespace WeatherInformationAcquisition.DataManage
{
    [Serializable]
    public class WeatherHour
    {
        private string rain;
        private string temp;
        private string time;

        [PropertyNickName("降雨量")]
        public string Rain { get => rain; set => rain = value; }
        [PropertyNickName("温度")]
        public string Temp { get => temp; set => temp = value; }
        [PropertyNickName("时间")]
        public string Time { get => time; set => time = value; }
        public WeatherHour()
        {

        }
        public override string ToString()
        {
            return ClassTools.ClassToString(this);
        }
    }
}
