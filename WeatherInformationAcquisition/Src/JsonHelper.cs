using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WeatherInformationAcquisition.DataManage;
using static WeatherInformationAcquisition.Src.ClassHelper;

namespace WeatherInformationAcquisition.Src
{
    public class JsonHelper
    {

        /// <summary>
        /// 解析Json数据
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static JObject ParseJson(string jsonText)
        {
            return JObject.Parse(jsonText);     
        }

        public static object CreateInstanceFromJObect(JObject json)
        {
            GetAllProperties(json);

            var dataClasslist = CreateInstanceFrom(data);

            List<object>  grouplist = DataToClass(dataClasslist);    
            
            return grouplist;
        }

        private static List<Tuple<string, string, string>> data = new List<Tuple<string, string, string>>();

        
        public static void GetAllProperties(JToken json)
        {
            foreach (var item in json.Children())
            {
                if (item.HasValues)
                {
                    GetAllProperties(item);
                }
                else
                {
                    //Tuple<string, string, string> tuple = Tuple.Create(item.Path, item.Value<string>(), item.Type.ToString());
                    Tuple<string, string, string> tuple = Tuple.Create(item.Path, item.Value<string>(), "System.String");

                    data.Add(tuple);
                }
            }

        }

        private static List<object> CreateInstanceFrom(List<Tuple<string, string, string>> data)
        {
            object result = default;
            List<CustPropertyInfo> lcpi = new List<CustPropertyInfo>();
            

            List<string> classNames = new List<string>();
            List<object> classes = new List<object>();


            //找到Data
            var dataInfo = data.Where(g => g.Item1.Split('.').Length == 3);

            var group = dataInfo.GroupBy(g => g.Item1.Split('.')[1]);

            foreach (var list in group)
            {
                foreach (var item in list)
                {
                    string[] strs = item.Item1.Split('.');
                    string proName = strs[2];
                    string type = item.Item3;

                    CustPropertyInfo cust = new CustPropertyInfo(type, proName);
                    lcpi.Add(cust);

                }
                string clName = list.ElementAt(0).Item1.Split('.')[1].Split('[')[0];
                object tempClass = ClassHelper.CreatInstance(clName, lcpi);         

                if (!classes.Any(g => g.GetType().Name == tempClass.GetType().Name))
                {
                    classes.Add(tempClass);
                }
    
                
                lcpi.Clear();
            }
            
            return classes;
        }
           
        private static List<object> DataToClass(List<object> clas)
        {
            List<object> result = new List<object>();
            //找到Data
            var dataInfo = data.Where(g => g.Item1.Split('.').Length == 3);

            var group = dataInfo.GroupBy(g => g.Item1.Split('.')[1]);

            foreach (var list in group)
            {
                object ins = clas.First(g => g.GetType().Name == list.ElementAt(0).Item1.Split('.')[1].Split('[')[0]);
                //重新实例化该对象
                object newIns = Activator.CreateInstance(ins.GetType());
                foreach (var item in list)
                {
                    var pros = newIns.GetType().GetProperties();
                    PropertyInfo pro = pros.First(g => g.Name == item.Item1.Split('.')[2]);
                    pro.SetValue(newIns, item.Item2);

                }
                result.Add(newIns);
            }
            return result;
        }

        public static T ParseJsonData<T>(string jsonData, Func<string, T> getData)
        {
            return getData(jsonData);
        }

        public static List<WeatherProvince> ParseProvince(string jsonData)
        {
            List<WeatherProvince> provinces = new List<WeatherProvince>();
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            dynamic pros = serializer.Deserialize<dynamic>(jsonData);

            foreach (var item in pros)
            {
                WeatherProvince province = new WeatherProvince();
                province.Code = item["code"];
                province.Name = item["name"];
                province.Url = item["url"];
                provinces.Add(province);
            }
            return provinces;
        }

     
        public static List<WeatherCity> ParseCity(string jsonData)
        {
            List<WeatherCity> cities = new List<WeatherCity>();
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            dynamic pros = serializer.Deserialize<dynamic>(jsonData);

            foreach (var item in pros)
            {
                WeatherCity city = new WeatherCity();
                city.Code = item["code"];
                city.City = item["city"];
                city.Province = item["province"];
                city.Url = item["url"];
                cities.Add(city);
            }
            return cities;
        }

        public static WeatherNMC ParseWeather(string jsonData)
        {
            WeatherNMC weather = new WeatherNMC();
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            dynamic pros = serializer.Deserialize<dynamic>(jsonData);

            dynamic data = pros["data"];
            dynamic real = data["real"];

            dynamic station = real["station"];

            weather.Province = station["province"];

            weather.City = station["city"];

            weather.PushTime = real["publish_time"];

            WeatherDayCondition condition = new WeatherDayCondition();

            dynamic con = real["weather"];

            condition.MaxTemp = con["temperature"].ToString();

            condition.MinTemp = con["temperatureDiff"].ToString();

            condition.Rain = con["rain"].ToString();

            condition.Info = con["info"];

            weather.WeatherDay = condition;

            dynamic tempchart = data["tempchart"];

            foreach (dynamic item in tempchart)
            {
                WeatherDay day = new WeatherDay();

                day.MaxTemp = item["max_temp"].ToString();
                day.MinTemp = item["min_temp"].ToString();
                day.Date = item["time"];

                weather.Weather14Days.Add(day);
            }

            dynamic passedchart = data["passedchart"];

            foreach (dynamic item in passedchart)
            {
                WeatherHour hour = new WeatherHour();

                hour.Rain = item["rain1h"].ToString();
                hour.Temp = item["temperature"].ToString();
                hour.Time = item["time"];

                weather.WeatherOneDay.Add(hour);
            }

            dynamic climate = data["climate"];
            dynamic months = climate["month"];

            foreach (dynamic item in months)
            {
                WeatherMonthHistory month = new WeatherMonthHistory();

                month.Rain = item["precipitation"].ToString();

                month.MaxTemp = item["maxTemp"].ToString();

                month.MinTemp = item["minTemp"].ToString();

                month.Month = item["month"].ToString();

                weather.WeatherHistory.Add(month);
            }
            return weather;
        }


    }
}
