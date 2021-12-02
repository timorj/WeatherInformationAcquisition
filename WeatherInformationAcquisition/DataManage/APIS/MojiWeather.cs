using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherInformationAcquisition.Properties;
using WeatherInformationAcquisition.Src;

namespace WeatherInformationAcquisition.DataManage.APIS
{
    public class MojiWeather:IWeatherRequest
    {
        private List<Tuple<string, string>> citiesData = new List<Tuple<string, string>>();

        private void GetCityID()
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

            List<Tuple<string, string>> cities = new List<Tuple<string, string>>();

            data.ForEach(g =>
            {
                var array = g.Split('\t');
                Tuple<string, string> temp = Tuple.Create(array[1], array[0]);
                cities.Add(temp);
            }
            );

            citiesData = cities;
        }

        private  Stream BytesToStream(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            return stream;
        }

        public (string, IWeather) WeatherForcastQuest(RequestParam param)
        {
            string jsonData = DataRequest.WeatherRequest(RequestType.Forecast, param.CityCode);

            JObject jobject = JsonHelper.ParseJson(jsonData);

            object result = JsonHelper.CreateInstanceFromJObect(jobject);

            Weather weather = new Weather(result);

            return (jsonData, weather);

        }

        /// <summary>
        /// 从城市列表中获取城市ID
        /// </summary>
        public string CityIDRequest(string cityName)
        {
            GetCityID();
            var city = citiesData.FirstOrDefault(g => g.Item1 == cityName);

            if (city != null)
                return city.Item2;

            return default;
    
        }

        public override string ToString()
        {
            return "墨迹天气";
        }
    }
    
}
