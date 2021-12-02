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
    public class WeatherProvince
    {
        private string code;
        private string name;
        private string url;

        [PropertyNickName("代码")]
        public string Code { get => code; set => code = value; }
        [PropertyNickName("名称")]
        public string Name { get => name; set => name = value; }

        [PropertyNickName("网址")]
        public string Url { get => url; set => url = value; }
        public WeatherProvince()
        {

        }
        public override string ToString()
        {
            return ClassTools.ClassToString(this);
        }
    }
}
