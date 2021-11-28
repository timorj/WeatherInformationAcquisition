using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherInformationAcquisition.DataManage
{
    public class Weather
    {
        public string cityId;
        public int temp;
        public string WD;  //风向  
        public string WS;     //风力  
        public string SD;  //相对湿度  
        public string time;//更新时间
    }
}
