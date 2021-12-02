using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeatherInformationAcquisition.Utils
{
    public class ClassTools
    {

        public static string ClassToString(object s)
        {
          
            string result = string.Empty;

            PropertyInfo[] origin = s.GetType().GetProperties();

            foreach (PropertyInfo prop in origin)
            {
                if (prop.PropertyType.IsGenericType)
                {
                    result += prop.GetCustomAttribute<PropertyNickNameAttribute>().NickName + "\r\n";
                    dynamic data = prop.GetValue(s, null);
                    foreach (var item in data)
                    {
                        result += item.ToString();
                    }

                }
                else
                {
                    result += prop.GetCustomAttribute<PropertyNickNameAttribute>().NickName + ":" + prop.GetValue(s).ToString() + "\r\n";
                }
            }

            return result;

        }

        
    }
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class PropertyNickNameAttribute : Attribute
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
