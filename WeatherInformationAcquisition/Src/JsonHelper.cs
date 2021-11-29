using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

    }
}
