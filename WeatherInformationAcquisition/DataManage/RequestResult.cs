using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WeatherInformationAcquisition.Src;
using WeatherInformationAcquisition.Utils;

namespace WeatherInformationAcquisition.DataManage
{
    public class RequestResult
    {
        private string forecastJsonData;

        private JObject forecastJobject;

        private object forecastClass;

        private IWeather forecastWeather;

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
        public IWeather ForecastWeather { get => forecastWeather; set => forecastWeather = value; }


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
           
                byte[] data = Encoding.UTF8.GetBytes(forecastWeather.ToString());
                stream.Write(data, 0, data.Length);
            }
        }

        public void ExportDataAsXML(string dir)
        {
            string fileName = Path.Combine(dir, "WeatherLog.xml");
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                XmlSerializer xz = new XmlSerializer(forecastWeather.GetType());
                xz.Serialize(sw, forecastWeather);
            }

        }

        public void ExportDataAsDocx(string dir)
        {
            string fileName = Path.Combine(dir, "WeatherLog.docx");
            string content = forecastWeather.ToString();

            WordHelper.CreatWordDocument(fileName, content);
        }


    }
}
