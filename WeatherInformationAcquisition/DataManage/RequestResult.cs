using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherInformationAcquisition.DataManage
{
    public class RequestResult
    {
        private string jsonData;

        /// <summary>
        /// 请求到的json数据
        /// </summary>
        public string JsonData { get => jsonData; set => jsonData = value; }
        public JObject Jobject { get => jobject; set => jobject = value; }
        public object Weather { get => weather; set => weather = value; }

        private JObject jobject;



        private object weather; 

    }
}
