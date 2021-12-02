using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WeatherInformationAcquisition.DataManage.APIS;
using WeatherInformationAcquisition.Properties;
using WeatherInformationAcquisition.Src;

namespace WeatherInformationAcquisition.DataManage
{
    public class WeatherRequestSystem
    {
        private static RequestParam param;
        public static RequestParam Param { get => param; set => param = value; }
        public static RequestResult Result { get => result; set => result = value; }


        private static RequestResult result;

        public static IWeatherRequest RequestMethod;

        static WeatherRequestSystem()
        {
            param = new RequestParam();
            result = new RequestResult();
        }

        #region 墨迹天气

        /// <summary>
        /// 从城市列表中获取城市ID
        /// </summary>
        public static void CityIDRequest()
        {
            param.CityCode = RequestMethod.CityIDRequest(param.City);
        }

        public static void WeatherForcastQuest() 
        {
            var forecast = RequestMethod.WeatherForcastQuest(Param);
            result.ForecastJsonData = forecast.Item1;
            result.ForecastWeather = forecast.Item2;
        }

        public static void WeatherConditionQuest()
        {
            
        }

        #endregion

       

    }
}
