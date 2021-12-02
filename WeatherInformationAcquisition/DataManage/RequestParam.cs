using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherInformationAcquisition.DataManage
{
    public class RequestParam
    {
 
        public string City { get => city; set => city = value; }

        private string city;

        private string cityCode;
        public string CityCode { get => cityCode; set => cityCode = value; }

        
        public RequestParam()
        {
            city = String.Empty;
            cityCode = String.Empty;
        }
    }
}
