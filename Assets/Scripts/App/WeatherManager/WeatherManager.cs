using UnityEngine;
using WeatherOrNot.Events.Weather;
using WeatherOrNot.Utils;

namespace WeatherOrNot.App.WeatherManager
{
    public class WeatherManager : MonoBehaviour
    {
        private WeatherTypes m_currentWeather;
        [SerializeField] private WeatherVisualController m_weatherVisualController;

        private void Awake()
        {
            EventBus.Subscribe<ChangeWeatherEvent>(OnWeatherChange);
        }

        private void OnWeatherChange(ChangeWeatherEvent args)
        {
            UpdateWeather(args.WeatherType);
        }

        private void UpdateWeather(WeatherTypes weather)
        {
            if (m_currentWeather == weather) return;

            m_currentWeather = weather;
            //TODO: Implement weather changes
            Debug.Log($"Weather changed to: {m_currentWeather}");
            EventBus.Notify(this, new UpdateWeatherEvent(m_currentWeather));
        }

    }
}