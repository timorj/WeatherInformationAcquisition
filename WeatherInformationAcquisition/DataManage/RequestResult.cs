using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeatherInformationAcquisition.DataManage
{
    public class RequestResult
    {
        private string forecastJsonData;

        private JObject forecastJobject;

        private object forecastClass;

        private Weather forecastWeather;

        /// <summary>
        /// 请求到的精简3天的json数据
        /// </summary>
        public string ForecastJsonData { get => forecastJsonData; set => forecastJsonData = value; }
        /// <summary>
        /// 请求到的精简3天的Jobject数据
        /// </summary>
        public JObject ForecastJobject { get => forecastJobject; set => forecastJobject = value; }

        /// <summary>
        /// 请求到的精简3天的数据
        /// </summary>
        public object ForecastClass { get => forecastClass; set => forecastClass = value; }

        /// <summary>
        /// 需要自定义获取的数据
        /// </summary>
        public Weather ForecastWeather { get => forecastWeather; set => forecastWeather = value; }


        private string conditionJsonData;

        private JObject conditionJobject;

        private object conditionClass;

        private Weather conditionWeather;

        /// <summary>
        /// 请求到的实况的json数据
        /// </summary>
        public string ConditionJsonData { get => conditionJsonData; set => conditionJsonData = value; }
        /// <summary>
        /// 请求到的实况的jobject数据
        /// </summary>
        public JObject ConditionJobject { get => conditionJobject; set => conditionJobject = value; }
      
        /// <summary>
        /// 请求到的实况数据
        /// </summary>
        public object ConditionClass { get => conditionClass; set => conditionClass = value; }

        /// <summary>
        /// 需要自定义请求的数据
        /// </summary>
        public Weather ConditionWeather { get => conditionWeather; set => conditionWeather = value; }

        public RequestResult()
        {

        }

        public void ExportDataAsTxt(string dir)
        {
            string fileName = Path.Combine(dir, "WeatherLog.txt");
            using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                PropertyInfo[] props = forecastWeather.GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    string line = prop.GetCustomAttribute<PropertyNickNameAttribute>().NickName + $"({prop.Name}):" + prop.GetValue(forecastWeather) + "\n";
                    byte[] data = Encoding.UTF8.GetBytes(line);
                    stream.Write(data, 0, data.Length);
                }
            }
        }

        public void ExportDataAsXML(string dir)
        {
            string fileName = Path.Combine(dir, "WeatherLog.xml");
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                XmlSerializer xz = new XmlSerializer(typeof(Weather));
                xz.Serialize(sw, forecastWeather);
            }

        }


    }
}
