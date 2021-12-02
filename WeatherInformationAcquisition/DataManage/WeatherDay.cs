using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherInformationAcquisition.Utils;

namespace WeatherInformationAcquisition.DataManage
{
    [Serializable]
    public class WeatherDay
    {
        private string date;
        private string maxTemp;
        private string minTemp;
        [PropertyNickName("日期")]
        public string Date { get => date; set => date = value; }
        [PropertyNickName("最高气温")]
        public string MaxTemp { get => maxTemp; set => maxTemp = value; }

        [PropertyNickName("最低气温")]
        public string MinTemp { get => minTemp; set => minTemp = value; }

        public WeatherDay()
        {

        }

        public override string ToString()
        {
            return ClassTools.ClassToString(this);
        }
    }
}
