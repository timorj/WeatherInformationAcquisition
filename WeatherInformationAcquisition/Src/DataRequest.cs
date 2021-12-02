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
using HtmlAgilityPack;

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

        private const string nmcHost = "http://www.nmc.cn";

        private const string nmcPath = "/publish/forecast/";

        /// <summary>
        /// 请求天气数据，并返回Joson数据
        /// </summary>
        /// <param name="cityID">城市ID,默认为2（北京市）</param>
        /// <returns>天气Json数据</returns>
        public static string WeatherRequest(RequestType requestType, string cityID)
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

        public static string WeatherRequestFromNMC(string cityID)
        {
            
            string url = "http://www.nmc.cn/rest/weather?stationid=" + cityID;

            return ParseHttpsData(url);
        }

        private static string ParseHttpsData(string url)
        {
            HttpWebResponse httpResponse = null;

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));


            return reader.ReadToEnd();
        }

        public static string ProvincesRequestFromNMC() 
        {
            string provinceUrl = "http://www.nmc.cn/rest/province/all";

            return ParseHttpsData(provinceUrl);
        
        }

        public static string CityCodeRequestFromNMC(string provinceCode)
        {
            string host1 = "http://www.nmc.cn/rest/province/";
            string path = provinceCode;
            string cityUrl = host1 + path;

            return ParseHttpsData(cityUrl);
        }

        public static string ParseHTMLText(string data)
        {
            string result = string.Empty;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(data);

            HtmlNode weatherNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='weather-header']");

            HtmlNodeCollection weatherItems = weatherNode.SelectNodes("//div[@class='real-item']");

            HtmlNode realRain = weatherItems[0].SelectSingleNode("//div[@id='realRain']");

            HtmlNode weatherForecastNode = weatherNode.SelectSingleNode("//div[@class='7days day7 pull-right clearfix']");

            HtmlNodeCollection weatherWrapNodes = weatherForecastNode.SelectNodes("//div[@class='weatherWrap']");

            return result;
        }
    
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
