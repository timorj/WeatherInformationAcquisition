using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeatherInformationAcquisition.DataManage
{
    [Serializable]
    public class Weather
    {
        private string name;
        private string pname;
        private string secondaryname;
        private string humidity;

        private string tempDay;
        private string tempNight;

        private string pop;

        private string conditionDay;

        /// <summary>
        /// 城市名称
        /// </summary>
        [PropertyNickName("城市名称")]
        public string Name { get => name; set => name = value; }  
        [PropertyNickName("省份名称")]
        /// <summary>
        /// 省份名称
        /// </summary>
        public string Pname { get => pname; set => pname = value; }

        [PropertyNickName("上级城市名称")]
        /// <summary>
        /// 上级城市名称
        /// </summary>
        public string Secondaryname { get => secondaryname; set => secondaryname = value; }

        [PropertyNickName("相对湿度")]
        /// <summary>
        /// 相对湿度（单位:%）
        /// </summary>
        public string Humidity { get => humidity; set => humidity = value; }

        [PropertyNickName("最高温度")]
        /// <summary>
        /// 白天温度（最高气温）
        /// </summary>
        public string TempDay { get => tempDay; set => tempDay = value; }

        [PropertyNickName("最低温度")]
        /// <summary>
        /// 夜间温度(最低气温)
        /// </summary>
        public string TempNight { get => tempNight; set => tempNight = value; }

        [PropertyNickName("降水概率")]
        /// <summary>
        /// 降水概率
        /// </summary>
        public string Pop { get => pop; set => pop = value; }

        [PropertyNickName("天气情况")]
        /// <summary>
        /// 白天天气情况
        /// </summary>
        public string ConditionDay { get => conditionDay; set => conditionDay = value; }

        public Weather()
        {

        }

        private List<PropertyInfo> properties = new List<PropertyInfo>();

        private void GetAllProperties(object data)
        {
            PropertyInfo[] props = data.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                object value = prop.GetValue(data, null);
                if(value.GetType().GetProperties().Length > 0)
                {
                    GetAllProperties(prop);
                }
                else
                {
                    properties.Add(prop);
                }
            }
        }

        public Weather(object weather)
        {            
            PropertyInfo[] origin = this.GetType().GetProperties();

            List<object> list = (List<object>)weather;

            Dictionary<PropertyInfo, object> props = new Dictionary<PropertyInfo, object>();
           
            foreach (object item in list)
            {
                var dest = item.GetType().GetProperties().ToList();
                
                dest.ForEach(g => {
                    if(!props.ContainsKey(g))
                    props.Add(g, item); });
        
            }

            foreach (PropertyInfo prop in origin)
            {
                if (props.Any(g => g.Key.Name.ToLower() == prop.Name.ToLower()))
                {
                    var item = props.First(g => g.Key.Name.ToLower() == prop.Name.ToLower());
                    prop.SetValue(this, item.Key.GetValue(item.Value));
                }
            }
          

        }

        public override string ToString()
        {
            string result = string.Empty;

            PropertyInfo[] origin = this.GetType().GetProperties();

            foreach (PropertyInfo prop in origin)
            {
                result += prop.GetCustomAttribute<PropertyNickNameAttribute>().NickName + ":" + prop.GetValue(this) + "\r\n";
            }

            return result;
        }
    }

    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class PropertyNickNameAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string nickName;

        // This is a positional argument
        public PropertyNickNameAttribute(string nickName)
        {
            this.nickName = nickName;

        }

        public string NickName
        {
            get { return nickName; }
        }

        // This is a named argument
        public int NamedInt { get; set; }
    }
}
