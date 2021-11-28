using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherInformationAcquisition.DataManage
{
    public class RequestParam
    {
 
        private int cityID;
        /// <summary>
        /// 请求的城市ID默认为2，北京市
        /// </summary>
        public int CityID { get => cityID; set => cityID = value; }
        public string City { get => city; set => city = value; }

        private string city;

        
        public RequestParam()
        {
            cityID = default;
            city = String.Empty;
        }
    }
}
