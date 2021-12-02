using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherInformationAcquisition.DataManage.APIS
{
    public interface IWeatherRequest
    {
        string CityIDRequest(string cityName);
        (string, IWeather) WeatherForcastQuest(RequestParam param);

    }
}
