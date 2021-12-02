using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherInformationAcquisition.Utils;

namespace WeatherInformationAcquisition.DataManage
{
    [Serializable]
    public class WeatherDayCondition
    {
        private string maxTemp;
        private string minTemp;
        private string rain;
        private string info;

        [PropertyNickName("最高气温")]
        public string MaxTemp { get => maxTemp; set => maxTemp = value; }
        [PropertyNickName("最低气温")]
        public string MinTemp { get => minTemp; set => minTemp = value; }
        [PropertyNickName("降雨量")]
        public string Rain { get => rain; set => rain = value; }

        [PropertyNickName("天气情况")]
        public string Info { get => info; set => info = value; }

        public WeatherDayCondition()
        {

        }
        public override string ToString()
        {
            return ClassTools.ClassToString(this);
        }
    }
}
