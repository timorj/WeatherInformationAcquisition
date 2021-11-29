using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Script.Serialization;
using WeatherInformationAcquisition.DataManage;

namespace WeatherInformationAcquisition.Src
{
    public enum RequestType
    {
        Forecast,
        Condition
    }
    class DataRequest
    {
        private const string host = "https://freecityid.market.alicloudapi.com";
        /// <summary>
        /// 精简预报3天
        /// </summary>
        private const string forecastPath = "/whapi/json/alicityweather/briefforecast3days";

        /// <summary>
        /// 精简实况
        /// </summary>
        private const string conditionPath = "/whapi/json/alicityweather/briefcondition";

        private const string method = "POST";
        //请求配置
        private const string appcode = "456e653469d74deda03dbf0dd81a4328";

        /// <summary>
        /// 请求天气数据，并返回Joson数据
        /// </summary>
        /// <param name="cityID">城市ID,默认为2（北京市）</param>
        /// <returns>天气Json数据</returns>
        public static string WeatherRequest(RequestType requestType, int cityID)
        {           
            string querys = "";
            
            string bodys = $"cityId={cityID}&token=677282c2f1b3d718152c4e25ed434bc4";
            string url = "";
            switch (requestType)
            {
                case RequestType.Forecast:
                    url = host + forecastPath;
                    break;
                case RequestType.Condition:
                    url = host + conditionPath;
                    break;
                default:
                    url = host + forecastPath;
                    break;
            }
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.Method = method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
            //根据API的要求，定义相对应的Content-Type
            httpRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            Console.WriteLine(httpResponse.StatusCode);
            Console.WriteLine(httpResponse.Method);
            Console.WriteLine(httpResponse.Headers);
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            return reader.ReadToEnd();

        }
    
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
