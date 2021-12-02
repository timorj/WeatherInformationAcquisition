using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeatherInformationAcquisition.Utils;

namespace WeatherInformationAcquisition.DataManage
{   
    [Serializable]
    public class WeatherCity
    {
        private string code;

        private string city;

        private string province;

        private string url;

        [PropertyNickName("代码")]
        public string Code { get => code; set => code = value; }
        [PropertyNickName("城市名称")]
        public string City { get => city; set => city = value; }
        [PropertyNickName("省份名称")]
        public string Province { get => province; set => province = value; }

        [PropertyNickName("网址")]
        public string Url { get => url; set => url = value; }

        public WeatherCity()
        {
                
        }

        public override string ToString()
        {
            return ClassTools.ClassToString(this);
        }

    }
}
