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
    public class NMCWeather:IWeatherRequest
    {
        private List<WeatherProvince> weatherProvinces = new List<WeatherProvince>();

        private List<WeatherCity> weatherCities = new List<WeatherCity>();

        private List<Tuple<string, string>> citiesData = new List<Tuple<string, string>>();

        /// <summary>
        /// 从城市列表中获取城市ID
        /// </summary>
        public string CityIDRequest(string cityName)
        {
            GetCityID();
            var city = citiesData.FirstOrDefault(g => g.Item1 == cityName);

            if (city != null)
                return city.Item2;
            return null;
        }

        private void GetCityID()
        {
            var excelData = Resources.cities;

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

            data.ForEach(g =>
            {
                var array = g.Split('\t');
                Tuple<string, string> temp = Tuple.Create(array[1], array[0]);
                citiesData.Add(temp);
            }
            );

        }
        private Stream BytesToStream(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            return stream;
        }
    

        public (string, IWeather) WeatherForcastQuest(RequestParam param)
        {
            string jsonData = DataRequest.WeatherRequestFromNMC(param.CityCode);

            WeatherNMC weather = JsonHelper.ParseJsonData<WeatherNMC>(jsonData, JsonHelper.ParseWeather);

            return (jsonData, weather);
        }
  

        #region 中央气象台
        public void GetAllCitiesCode()
        {
            string provincesJsonData = DataRequest.ProvincesRequestFromNMC();

            weatherProvinces = JsonHelper.ParseJsonData<List<WeatherProvince>>(provincesJsonData, JsonHelper.ParseProvince);

            foreach (WeatherProvince province in weatherProvinces)
            {
                string cityJsonData = DataRequest.CityCodeRequestFromNMC(province.Code);
                List<WeatherCity> cities = JsonHelper.ParseJsonData<List<WeatherCity>>(cityJsonData, JsonHelper.ParseCity);

                weatherCities.AddRange(cities);
            }

        }

        public void SaveCitiesCode()
        {
            if (weatherCities == null) return;

            string filename = "cities.txt";
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("城市代码\t城市名称");
                foreach (var item in weatherCities)
                {
                    sw.WriteLine(item.Code + "\t" + item.City);
                }
                sw.Close();
            }
        }





        #endregion

        public override string ToString()
        {
            return "中央气象台";
        }
    }
}
