using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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

        private static List<Tuple<string, int>> citiesData;      

        static WeatherRequestSystem()
        {
            param = new RequestParam();
            result = new RequestResult();
        }

        /// <summary>
        /// 从城市列表中获取城市ID
        /// </summary>
        public static void CityIDRequest()
        {
            GetCityID();

            var city = citiesData.FirstOrDefault(g => g.Item1 == Param.City);
            
            if(city != null)
            Param.CityID = city.Item2;
           

        }


        private static void GetCityID()
        {
            var excelData = Resources.城市ID列表;

            byte[] excels = Encoding.UTF8.GetBytes(excelData);

            Stream stream = BytesToStream(excels);
            StreamReader reader = new StreamReader(stream);
            List<string> data = new List<string>();
            while (!reader.EndOfStream)
            {
                data.Add(reader.ReadLine());
            }
            reader.Close();

            //去掉表头
            data = data.Skip(1).ToList();

            List<Tuple<string, int>> cities = new List<Tuple<string, int>>();

            data.ForEach(g =>
            {
                var array = g.Split('\t');
                Tuple<string, int> temp = Tuple.Create(array[1], int.Parse(array[0]));
                cities.Add(temp);
            }
            );

            citiesData = cities;
        }

        private static Stream BytesToStream(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            return stream;
        }
        
        public static void WeatherForcastQuest()
        {
            result.ForecastJsonData = DataRequest.WeatherRequest(RequestType.Forecast, param.CityID);

            result.ForecastJobject = JsonHelper.ParseJson(result.ForecastJsonData);

            result.ForecastClass = JsonHelper.CreateInstanceFromJObect(result.ForecastJobject);

            

            result.ForecastWeather = new Weather(result.ForecastClass);

            
        }

        public static void WeatherConditionQuest()
        {
            result.ConditionJsonData = DataRequest.WeatherRequest(RequestType.Condition, param.CityID);

            result.ConditionJobject = JsonHelper.ParseJson(result.ConditionJsonData);

            result.ConditionClass = JsonHelper.CreateInstanceFromJObect(result.ConditionJobject);

            result.ConditionWeather = new Weather(result.ConditionWeather);
        }

        

    }
}
