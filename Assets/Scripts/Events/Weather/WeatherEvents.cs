using WeatherOrNot.Utils;

namespace WeatherOrNot.Events.Weather
{
    public class ChangeWeatherEvent
    {
        public WeatherTypes WeatherType { get; private set; }

        public ChangeWeatherEvent(WeatherTypes weatherType)
        {
            WeatherType = weatherType;
        }
    }

    public class UpdateWeatherEvent
    {
        public WeatherTypes WeatherType { get; private set; }

        public UpdateWeatherEvent(WeatherTypes weatherType)
        {
            WeatherType = weatherType;
        }
    }
}